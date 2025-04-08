using Blok3Game.Engine.AssetHandler;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Blok3Game.Engine.GameObjects;

namespace Blok3Game.GameStates
{
    public class LobbyWaitForPlayersState : MovableMenuItem
    {
        private Dictionary<string, TextGameObject> players = new Dictionary<string, TextGameObject>();

        public LobbyWaitForPlayersState() : base()
        {
            CreateButtons();
        }

        private void CreateButtons()
        {
            Button buttonStart = CreateButton(new Vector2(150, 525), "START GAME", OnButtonStartClicked);
            Button buttonCancel = CreateButton(new Vector2(450, 525), "CANCEL", OnButtonCancelClicked);
        }

        protected override void HandleIncomingMessages()
        {
            base.HandleIncomingMessages();

            SocketClient.Instance.SubscribeToDataPacket<EnterRoomData>(OnPlayerEnteredRoomDataReceived);
            SocketClient.Instance.SubscribeToDataPacket<RoomActivePlayersData>(OnActivePlayersDataReceived);
            SocketClient.Instance.SubscribeToDataPacket<StartGameData>(OnGameStartedDataReceived);

            //We need to remove the player from the list when they leave the room.
            //This can either be because they left the room or because they disconnected.
            SocketClient.Instance.SubscribeToDataPacket<LeaveRoomData>(OnPlayerLeftRoom);
            SocketClient.Instance.SubscribeToDataPacket<PlayerDisconnectedData>(OnPlayerDisconnected);
            SocketClient.Instance.SubscribeToDataPacket<PlayerReconnectedData>(OnPlayerReconnectedDataReceived);
        }

        private void AddPlayer(PlayerData player)
        {
            string userId = player.UserId;

            if (players.ContainsKey(userId))
            {
                return;
            }

            string playerName = player.Name.ToUpper();

            TextGameObject textGameObject = CreateText(new Vector2(211, 250 + 50 * players.Count), playerName);
            players.Add(userId, textGameObject);
        }

        private void RemovePlayer(string userId)
        {
            if (players.ContainsKey(userId))
            {
                Remove(players[userId]);
                players.Remove(userId);
            }
        }

        private void OnPlayerLeftRoom(LeaveRoomData leaveRoomData)
        {
            string userId = leaveRoomData.Player.UserId;

            if (userId == SocketClient.Instance.UserId)
            {
                //for every player in the room, remove them from the list.
                foreach (KeyValuePair<string, TextGameObject> player in players)
                {
                    RemovePlayer(player.Key);
                }
            }
            else
            { 
                RemovePlayer(userId);            
            }
        }

        private void OnPlayerDisconnected(PlayerDisconnectedData playerDisconnectedData)
        {
            string userId = playerDisconnectedData.Player.UserId;
            RemovePlayer(userId);
        }

        private void OnActivePlayersDataReceived(RoomActivePlayersData data)
        {
            foreach (PlayerData player in data.Players)
            {
                AddPlayer(player);
            }
        }

        private void OnPlayerEnteredRoomDataReceived(EnterRoomData enterRoomData)
        {
            AddPlayer(enterRoomData.Player);
        }

        private void OnPlayerReconnectedDataReceived(PlayerReconnectedData playerReconnectedData)
        { 
            AddPlayer(playerReconnectedData.Player);
        }

        public override void Reset()
        {
            base.Reset();

            SocketClient.Instance.SendDataPacket(new RoomActivePlayersData());
        }

        private void OnGameStartedDataReceived(StartGameData data)
        {
            nextScreenName = GameStateManager.GAME_STATE;
            Animate(AnimationState.MovingOffscreenInactive);
        }

        private void OnButtonStartClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_agree");
            SocketClient.Instance.SendDataPacket(new StartGameData()
            {
                RoomId = SocketClient.Instance.RoomId,
                UserId = SocketClient.Instance.UserId
            });
        }

        private void OnButtonCancelClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_cancel");
            nextScreenName = GameStateManager.GO_TO_PREVIOUS_SCREEN;
            SocketClient.Instance.SendDataPacket(new LeaveRoomData());
            Animate(AnimationState.MovingOffscreenInactive);
        }
    }   
}
