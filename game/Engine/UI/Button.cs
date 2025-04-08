using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;

namespace Blok3Game.Engine.UI
{
    public class Button : UIElement
    {
        public SpriteSheet Sprite => background.Sprite;
        private SpriteGameObject background;
        private TextGameObject text;        

        public bool Disabled { get; set; }

        public Button(Vector2 position, float scale, string imageName) : base()
        {
            Position = position;

            AddBackground(scale, imageName);
            AddText();

            UIElementState = UIElementMouseState.Normal;
        }

        private void AddBackground(float scale, string imageName)
        {
            background = new SpriteGameObject($"Images/UI/{imageName}", 0, "button");
            background.Scale = scale;
            AddBackground(background);            
        }

        private void AddText()
        {
            text = new TextGameObject("Fonts/SpriteFont", 1, "text");
            Add(text);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            if (Disabled)
            {
                UIElementState = UIElementMouseState.Disabled;
                return;
            }

            if (background.BoundingBox.Contains(inputHelper.MousePosition))
            {
                if (inputHelper.MouseLeftButtonPressed)
                {
                    isClicked = true;
                }
                if (isClicked)
                {
                    UIElementState = UIElementMouseState.Pressed;
                }
                else
                {
                    UIElementState = UIElementMouseState.Hover;
                }
                if (isClicked && inputHelper.MouseLeftButtonReleased)
                {
                    isClicked = false;
                    OnClicked();
                }
            }
            else
            {
                UIElementState = UIElementMouseState.Normal;
                isClicked = false;
            }
        }

        public string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                //use text width and height to center text
                text.Text = value;
                text.Position = new Vector2(
                    background.Width / 2 - text.Size.X / 2,
                    background.Height / 2 - text.Size.Y / 2);
            }
        }
    }
}