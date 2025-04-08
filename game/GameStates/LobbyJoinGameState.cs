using Blok3Game.Engine.AssetHandler;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;

namespace Blok3Game.GameStates
{
    public class LobbyJoinGameState : MovableMenuItem
    {
        private TextGameObject title;
        private TextInput playerNameInput;
		private Button buttonJoin;

        public LobbyJoinGameState() : base()
        {
            playerNameInput = CreateTextInputField(new Vector2(211, 350), "ENTER YOUR NAME");
            CreateButtons();
        }

		protected override void HandleIncomingMessages()
		{
			base.HandleIncomingMessages();

			SocketClient.Instance.SubscribeToDataPacket<EnterRoomData>(OnEnterRoomData);
			SocketClient.Instance.SubscribeToDataPacket<StartGameData>(OnStartGameDataReceived);
			SocketClient.Instance.SubscribeToDataPacket<RemoveRoomData>(OnRoomRemovedDataReceived);
		}

		private void OnRoomNoLongerAvailable(string roomId)
		{
			//if the room we were trying to join is no longer available
			if (roomId == SocketClient.Instance.RoomId)
			{
				//disable the join button, so the user can't try to join again.
				buttonJoin.Disabled = true;

				//display an error message to the user.
				DisplayErrorMessage("Room no longer available", () =>
				{
					//once the error message is disapeared, go back to the previous screen.
					nextScreenName = GameStateManager.LOBBY_JOIN_OR_CREATE_STATE;
					Animate(AnimationState.MovingOffscreenInactive);
				});
			}
		}

		private void OnStartGameDataReceived(StartGameData data)
		{
			OnRoomNoLongerAvailable(data.RoomId);
		}

		private void OnRoomRemovedDataReceived(RemoveRoomData data)
		{
			OnRoomNoLongerAvailable(data.RoomId);
		}

		private void OnEnterRoomData(EnterRoomData data)
		{
			if (data.Player.UserId == SocketClient.Instance.UserId)
			{
				nextScreenName = GameStateManager.LOBBY_WAIT_FOR_PLAYERS_STATE;
				Animate(AnimationState.MovingOffscreenInactive);
			}
		}

		private void CreateButtons()
        {
            buttonJoin = CreateButton(new Vector2(150, 525), "JOIN", OnButtonJoinClicked);
            CreateButton(new Vector2(450, 525), "CANCEL", OnButtonCancelClicked);
        }

        private void OnButtonJoinClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_agree");
            SocketClient.Instance.SendDataPacket(new EnterRoomData()
            {
                RoomId = SocketClient.Instance.RoomId,
                Player = new PlayerData()
                {
                    Name = playerNameInput.Text
                }
            });
        }

        private void OnButtonCancelClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_cancel");
            nextScreenName = GameStateManager.GO_TO_PREVIOUS_SCREEN;
            Animate(AnimationState.MovingOffscreenInactive);
        }

        public override void Reset()
        {
            base.Reset();

            if (title != null)
            {
                Remove(title);
            }
            title = CreateText(new Vector2(211, 200), $"JOIN ROOM {SocketClient.Instance.RoomId}");
        }
    }
}
