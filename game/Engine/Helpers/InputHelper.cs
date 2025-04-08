using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Blok3Game.Engine.Helpers
{
	public class InputHelper
	{
		protected MouseState currentMouseState, previousMouseState;
		protected KeyboardState currentKeyboardState, previousKeyboardState;
        public KeyboardState CurrentKeyboardState => currentKeyboardState;
        public KeyboardState PreviousKeyboardState => previousKeyboardState;
		protected Vector2 scale, offset;

		public InputHelper()
		{
			scale = Vector2.One;
			offset = Vector2.Zero;
		}

		public void Update()
		{
			previousMouseState = currentMouseState;
			previousKeyboardState = currentKeyboardState;
			currentMouseState = Mouse.GetState();
			currentKeyboardState = Keyboard.GetState();
		}

		public Vector2 Scale
		{
			get { return scale; }
			set { scale = value; }
		}

		public Vector2 Offset
		{
			get { return offset; }
			set { offset = value; }
		}

		public Vector2 MousePosition
		{
			get { return (new Vector2(currentMouseState.X, currentMouseState.Y) - offset ) / scale; }
		}

        public bool ShiftKeyDown => currentKeyboardState.IsKeyDown(Keys.LeftShift) || currentKeyboardState.IsKeyDown(Keys.RightShift);

		public bool MouseLeftButtonPressed => 
			currentMouseState.LeftButton == ButtonState.Pressed &&
			previousMouseState.LeftButton == ButtonState.Released;

		public bool MouseLeftButtonReleased => 
			currentMouseState.LeftButton == ButtonState.Released && 
			previousMouseState.LeftButton == ButtonState.Pressed;    

		public bool MouseLeftButtonDown => 
			currentMouseState.LeftButton == ButtonState.Pressed;

		public bool KeyPressed(Keys k)
		{
			return currentKeyboardState.IsKeyDown(k) && previousKeyboardState.IsKeyUp(k);
		}

		public bool IsKeyDown(Keys k)
		{
			return currentKeyboardState.IsKeyDown(k);
		}

		public bool AnyKeyPressed => 
			currentKeyboardState.GetPressedKeys().Length > 0 && 
			previousKeyboardState.GetPressedKeys().Length == 0;

		public bool GetKeyPressed(out Keys key)
		{
			if (currentKeyboardState.GetPressedKeys().Length > 0)
			{
				key = currentKeyboardState.GetPressedKeys()[0];
				return true;
			}
			else
			{
				key = Keys.None;
				return false;
			}
		}
	}
}