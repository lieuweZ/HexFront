using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Blok3Game.GameStates
{
    public class MovableMenuItem : GameObjectList
    {
        public enum AnimationState
        {
            IdleActive,
            IdleOffscreenInactive,
            MovingInscreenInactive,
            MovingOffscreenInactive,
        }
        private SpriteGameObject background;
        private const float ANIMATION_SPEED = 20f;
        protected const float BUTTON_SCALE = 0.2f;

        protected AnimationState animationState;
        protected List<Button> buttons;
        protected List<TextInput> textInputs;
        protected List<TextGameObject> texts;
        protected string nextScreenName;

        public event Action OnActive;
        public event Action OnOffscreen;

        public MovableMenuItem() : base()
        {            
            InitializeVisualElements();

            HandleIncomingMessages();

            OnActive += OnMenuItemStateActive;
            OnOffscreen += OnMenuItemOffscreen;
        }

        private void InitializeVisualElements()
        {
            CreateBackground();

            buttons = new List<Button>();
            textInputs = new List<TextInput>();
            texts = new List<TextGameObject>();
        }

        protected virtual void HandleIncomingMessages()
        {
            //override this method to handle incoming messages.
			//use the following code to subscribe to a message:
			SocketClient.Instance.SubscribeToDataPacket<ErrorData>(OnErrorMessageDataReceived);
        }

		private void OnErrorMessageDataReceived(ErrorData data)
		{
			if (IsActive)
			{
				DisplayErrorMessage(data.Reason);
			}
		}

		protected void DisplayErrorMessage(string message, Action onMessageDisappeared = null)
		{
			ErrorMessage errorMessage = new ErrorMessage(message);
			errorMessage.Position = new Vector2() 
			{
				X = (GameEnvironment.Screen.X - errorMessage.Size.X) / 2,
				Y = (GameEnvironment.Screen.Y - errorMessage.Size.Y) / 2 - 150
			};

			errorMessage.OnTimerEnd += (ErrorMessage errorMessage) => 
			{
				Remove(errorMessage);
				onMessageDisappeared?.Invoke();
			};
			Add(errorMessage);
		}

        protected void AddButton(Button button)
        {
            Add(button);
            buttons.Add(button);
        }

        protected void AddTextInput(TextInput textInput)
        { 
            Add(textInput);
            textInputs.Add(textInput);
        }

        protected void AddText(TextGameObject text)
        {
            Add(text);
            texts.Add(text);
        }

        protected virtual void OnMenuItemStateActive()
        {
            foreach (Button button in buttons)
            {
                button.Disabled = false;
            }
        }

        protected virtual void OnMenuItemOffscreen()
        {
            foreach (Button button in buttons)
            {
                button.Disabled = true;
            }

            GameEnvironment.GameStateManager.SwitchTo(nextScreenName);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (animationState)
            { 
                case AnimationState.IdleActive:
                case AnimationState.IdleOffscreenInactive:
                    //do nothing
                    break;
                case AnimationState.MovingInscreenInactive:                    
                    MoveToCenter();
                    break;
                case AnimationState.MovingOffscreenInactive:                   
                    MoveToTop();
                    break;
            }
        }

        private void MoveToCenter()
        {
            if (Position.Y < (GameEnvironment.Screen.Y - background.Height) / 2)
            {
                Position += Vector2.UnitY * ANIMATION_SPEED;
            }
            else
            {
                animationState = AnimationState.IdleActive;
                OnActive?.Invoke();
            }
        }

        private void MoveToTop()
        {
            if (Position.Y > -background.Height)
            {
                Position -= Vector2.UnitY * ANIMATION_SPEED;
            }
            else
            {
                animationState = AnimationState.IdleOffscreenInactive;
                OnOffscreen?.Invoke();
            }
        }

        public void Animate(AnimationState animationState)
        {
            this.animationState = animationState;
        }

        private void CreateBackground()
        {
            background = new SpriteGameObject("Images/UI/Frame", 0, "background");
            background.Scale = 0.3f;

            //use the width and height of the background to position it in the center of the screen
            background.Position = new Vector2((GameEnvironment.Screen.X - background.Width) / 2,
                (GameEnvironment.Screen.Y - background.Height) / 2 - 50);
            Add(background);
        }

        public override void Reset()
        {
            base.Reset();
            animationState = AnimationState.IdleOffscreenInactive;
            Position = new Vector2(0, -background.Height);
            Animate(AnimationState.MovingInscreenInactive);
        }

        protected TextGameObject CreateText(Vector2 position, string text)
        {
            TextGameObject textObject = new TextGameObject("Fonts/SpriteFont", 1, "text");
            textObject.Position = position;
            textObject.Color = Color.Black;
            textObject.Text = text;
            Add(textObject);
            return textObject;
        }

        protected TextInput CreateTextInputField(Vector2 position, string accompanyingText)
        {
            CreateText(position - Vector2.UnitY * 20, accompanyingText);
            TextInput textInput = new TextInput(position, 0.2f);
            AddTextInput(textInput);
            return textInput;
        }
        protected Button CreateButton(Vector2 position, string text, Action<UIElement> onPressed, float scale = BUTTON_SCALE, string imageName = "Button@1x4")
        {
            Button button = new Button(position, scale, imageName);
            button.Text = text;
            button.Clicked += onPressed;
            AddButton(button);
            return button;
        }

        public bool IsActive => animationState == AnimationState.IdleActive;
    }
}
