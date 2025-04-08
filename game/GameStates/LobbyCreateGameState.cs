using Blok3Game.Engine.AssetHandler;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;

namespace Blok3Game.GameStates
{
    public class LobbyCreateGameState : MovableMenuItem
    {
        TextInput gameNameInput, playerNameInput;
        public LobbyCreateGameState() : base()
        {
            playerNameInput = CreateTextInputField(new Vector2(211, 250), "ENTER YOUR NAME");
            gameNameInput = CreateTextInputField(new Vector2(211, 350), "ENTER GAME NAME");
            
            playerNameInput.Clicked += OnTextInputClicked;
            gameNameInput.Clicked += OnTextInputClicked;

            gameNameInput.UIElementState = UIElementMouseState.Disabled;
            CreateButtons();
        }

        private void OnRoomCreatedDataReceived(CreateRoomData data)
        {
			string userId = SocketClient.Instance.UserId;
            SocketClient.Instance.RoomId = data.RoomId;
            if (data.Author == userId)
            {
                SocketClient.Instance.SendDataPacket(new EnterRoomData()
                {                    
                    RoomId = data.RoomId,
                    Player = new PlayerData()
                    {
                        Name = playerNameInput.Text,
						UserId = userId
                    }
                });
                nextScreenName = GameStateManager.LOBBY_WAIT_FOR_PLAYERS_STATE;
                Animate(AnimationState.MovingOffscreenInactive);
            }
        }

        protected override void HandleIncomingMessages()
        {
			base.HandleIncomingMessages();
            SocketClient.Instance.SubscribeToDataPacket<CreateRoomData>(OnRoomCreatedDataReceived);
        }

        private void CreateButtons()
        { 
            Button buttonCreate = CreateButton(new Vector2(150, 525), "CREATE GAME", OnButtonCreateClicked);
            Button buttonCancel = CreateButton(new Vector2(450, 525), "CANCEL", OnButtonCancelClicked);
        }

        private void OnButtonCreateClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_agree");
            SocketClient.Instance.SendDataPacket(new CreateRoomData() 
            {
                RoomId = gameNameInput.Text 
            });
        }

        private void OnButtonCancelClicked(UIElement element)
        {
			GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("button_cancel");
            nextScreenName = GameStateManager.GO_TO_PREVIOUS_SCREEN;
            Animate(AnimationState.MovingOffscreenInactive);
        }

        private void OnTextInputClicked(UIElement uiElement)
        {
            foreach (TextInput textInput in textInputs)
            {
                textInput.UIElementState = UIElementMouseState.Disabled;
            }

            uiElement.UIElementState = UIElementMouseState.Normal;
        }
    }
}
