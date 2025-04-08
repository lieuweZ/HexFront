using Blok3Game.Engine.AssetHandler;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Blok3Game.GameStates
{
    public class LobbyJoinOrCreateState : MovableMenuItem
    {
        private Dictionary<string, Button> activeRooms;

        /**
         * we need to queue the rooms that are created while listening to the server because
         * we can't add them to the draw list from a different thread.
         */
        private Queue<CreateRoomData> createRoomsDuringNextUICycle;

        public LobbyJoinOrCreateState() : base()
        {   
            CreateButtons();
            createRoomsDuringNextUICycle = new Queue<CreateRoomData>();
            activeRooms = new Dictionary<string, Button>();

			//AudioManager.Instance.PlaySong("main_menu", true);
        }

        protected override void HandleIncomingMessages()
        {
			base.HandleIncomingMessages();
            SocketClient.Instance.SubscribeToDataPacket<CreateRoomData>(OnRoomCreatedDataReceived);
            SocketClient.Instance.SubscribeToDataPacket<ActiveRoomsData>(OnActiveRoomsDataReceived);
            SocketClient.Instance.SubscribeToDataPacket<RemoveRoomData>(OnRoomRemovedDataReceived);
			SocketClient.Instance.SubscribeToDataPacket<StartGameData>(OnStartGameDataReceived);
        }

        public override void Reset()
        {
            base.Reset();
            SocketClient.Instance.SendDataPacket(new ActiveRoomsData());
        }      

        private void AddRoomInfo(CreateRoomData room)
        {   
            createRoomsDuringNextUICycle.Enqueue(room);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            while (createRoomsDuringNextUICycle.Count > 0)
            {
                CreateRoomData room = createRoomsDuringNextUICycle.Dequeue();
                if (!activeRooms.ContainsKey(room.RoomId))
                {
                    Button button = CreateButton(
                        new Vector2(211, 200 + 60 * activeRooms.Count), 
                        room.RoomId, 
                        OnEnterRoomButtonClicked, 
                        BUTTON_SCALE, 
                        "Button_Big@1x4");
					activeRooms.Add(room.RoomId, button);
                }
            }
        }

        private void OnRoomCreatedDataReceived(CreateRoomData createRoomData)
        {
            AddRoomInfo(createRoomData);
        }

        private void OnActiveRoomsDataReceived(ActiveRoomsData activeRoomsData)
        {
			if (activeRoomsData.Rooms != null)
			{
				foreach (CreateRoomData room in activeRoomsData.Rooms)
				{
					AddRoomInfo(room);
				}
			}
        }

		private void RemoveRoomFromList(string roomId)
		{
			//search the dictionary for the room with the same id and remove it.
			if (activeRooms.ContainsKey(roomId))
			{
				Button button = activeRooms[roomId];
				Remove(button);
			}
		}

		private void OnStartGameDataReceived(StartGameData startGameData)
		{
			RemoveRoomFromList(startGameData.RoomId);
		}

        private void OnRoomRemovedDataReceived(RemoveRoomData removeRoomData)
        {
            RemoveRoomFromList(removeRoomData.RoomId);
        }

        private void CreateButtons() 
        {
            CreateButton(new Vector2(150, 525), "CREATE GAME", OnButtonCreateClicked);
        }

        private void OnButtonCreateClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_agree");
            nextScreenName = GameStateManager.LOBBY_CREATE_GAME_STATE;
            Animate(AnimationState.MovingOffscreenInactive);
        }

        private void OnEnterRoomButtonClicked(UIElement uiElement)
        {
            Button button = uiElement as Button;
            string roomid = button.Text;
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_agree");
            if (roomid != null) 
            {
                SocketClient.Instance.RoomId = roomid;
                nextScreenName = GameStateManager.LOBBY_JOIN_GAME_STATE;
                Animate(AnimationState.MovingOffscreenInactive);
            }
        }
    }
}
