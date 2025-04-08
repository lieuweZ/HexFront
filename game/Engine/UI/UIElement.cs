using Blok3Game.Engine.GameObjects;
using System;

namespace Blok3Game.Engine.UI
{
    public enum UIElementMouseState
    {
        Disabled    = 0,
        Normal      = 1,
        Hover       = 2,
        Pressed     = 3
    }

    public class UIElement : GameObjectList
    {
        public virtual UIElementMouseState UIElementState
        {
            get
            {
                return uiElementMouseState;
            }
            set
            {
                uiElementMouseState = value;
                background.Sprite.SheetIndex = (int)value;
            }
        }

        protected UIElementMouseState uiElementMouseState;

        public event Action<UIElement> Clicked;
        
        private SpriteGameObject background;
        protected bool isClicked = false;

        protected void AddBackground(SpriteGameObject background)
        {
            this.background = background;
            Add(background);
        }

        protected void OnClicked()
        {
            Clicked?.Invoke(this);
        }
    }
}
