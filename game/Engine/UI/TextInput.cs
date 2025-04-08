using System.Collections.Generic;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Blok3Game.Engine.UI
{
    //Updated with special thanks to Amine El Hammdaoui
    public class TextInput : UIElement
    {
        // Map of special characters that can be entered with and without shift
        private Dictionary<int, char> normalChars = new Dictionary<int, char> 
        {
            {32, ' '}, {48, '0'}, {49, '1'}, {50, '2'}, {51, '3'}, {52, '4'}, {53, '5'},
            {54, '6'}, {55, '7'}, {56, '8'}, {57, '9'}, {189, '-'}, {188, ','},
            {190, '.'}, {191, '/'}
        };

        private Dictionary<int, char> shiftChars = new Dictionary<int, char> 
        {
            {32, ' '}, {49, '!'}, {50, '@'}, {51, '#'}, {52, '$'}, {53, '%'}, {54, '^'},
            {55, '&'}, {56, '*'}, {57, '('}, {48, ')'}, {189, '_'}, {188, '<'},
            {190, '>'}, {191, '?'}
        };

        private SpriteGameObject background;
        private TextGameObject text;
        
        public int CharacterLimit { get; set; } = 40;

        public override UIElementMouseState UIElementState 
        { 
            get => base.UIElementState;
            set 
            {
                uiElementMouseState = value;
                background.Sprite.SheetIndex = (int)value;
            }
        }

        public TextInput(Vector2 position, float scale) : base()
        {
            Position = position;

            AddBackground(scale);
            AddText();

            UIElementState = UIElementMouseState.Normal;
        }

        private void AddBackground(float scale)
        {
            background = new SpriteGameObject("Images/UI/TextInput@1x2", 0, "background");
            background.Scale = scale;
            AddBackground(background);
        }

        private void AddText()
        {
            text = new TextGameObject("Fonts/SpriteFont", 1, "text");
            text.Text = "";
            text.Position = new Vector2(20, background.Height / 2 - text.Size.Y / 2 - 5);
            Add(text);
        }

        private void HandleTextSelection(InputHelper inputHelper)
        {
            // Selecteren van de tekstbalk
            if (background.BoundingBox.Contains(inputHelper.MousePosition))
            {
                if (inputHelper.MouseLeftButtonPressed)
                {
                    isClicked = true;
                }
                
                if (isClicked && inputHelper.MouseLeftButtonReleased) 
                {
                    isClicked = false;
                    OnClicked();
                }
            }
        }

        private void HandleBackspace()
        {
            if (text.Text.Length > 0)
            {
                text.Text = text.Text.Substring(0, text.Text.Length - 1);
            }
        }

        private void HandleCharacterInput(InputHelper inputHelper) 
        {
            KeyboardState currentKeyboardState = inputHelper.CurrentKeyboardState;
            KeyboardState previousKeyboardState = inputHelper.PreviousKeyboardState;

            Keys[] pressedKeys = currentKeyboardState.GetPressedKeys();
            foreach (Keys key in pressedKeys) 
            {
                if (!previousKeyboardState.IsKeyDown(key)) 
                {
                    ProcessKey(key, inputHelper.ShiftKeyDown);
                }
            }
        }

        private void ProcessKey(Keys key, bool shiftIsDown) 
        {
            int keyValue = (int)key;

            if (keyValue >= 65 && keyValue <= 90) 
            { 
                // Letters
                text.Text += shiftIsDown ? key.ToString().ToUpper() : key.ToString().ToLower();
            } else 
            {
                char? character = GetCharacterForKey(keyValue, shiftIsDown);
                text.Text += character?.ToString();
            }
        }

        private char? GetCharacterForKey(int keyValue, bool shiftIsDown)
        {            
            if (shiftIsDown && shiftChars.ContainsKey(keyValue)) 
            {
                return shiftChars[keyValue];
            } else if (!shiftIsDown && normalChars.ContainsKey(keyValue)) 
            {
                return normalChars[keyValue];
            }

            return null;
        }
        
        public override void HandleInput(InputHelper inputHelper) 
        {
            base.HandleInput(inputHelper);
            HandleTextSelection(inputHelper);

            // Cancel all input when the textbox is not enabled
            if (UIElementState == UIElementMouseState.Disabled) 
            {
                return;
            }

            if (text.Text.Length >= CharacterLimit)
            {
                return;
            }

            if (inputHelper.KeyPressed(Keys.Back)) 
            {
                HandleBackspace();
            }

            HandleCharacterInput(inputHelper);
        }
        public string Text => text.Text;
    }
}
