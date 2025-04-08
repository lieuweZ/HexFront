using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public interface IGameLoopObject
	{
		void HandleInput(InputHelper inputHelper);

		void Update(GameTime gameTime);

		void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		void Reset();
	}
}