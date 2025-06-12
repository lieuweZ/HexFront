using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class FancyTextObject : TextGameObject
    {
		private long life = 0;
		public FancyTextObject(Vector2 pos, string text, long life, string assetname = "Fonts/SpriteFont", int layer = 0, string id = "")
			: base(assetname, layer, id)
		{
			this.position = pos;
			this.life = life;
			this.text = text;
        }

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (visible && text != null)
			{
				spriteBatch.DrawString(spriteFont, text, GlobalPosition, color);
			}
		}

        public override void Update(GameTime gameTime)
        {
			life--;

            this.position = this.position + new Vector2(0, 1);
			if(this.parent != null)
			if(life <= 0 && (this.parent.GetType() == typeof(GameObjectList) || this.parent.GetType() == typeof(GameState)))
			{
                    ((GameObjectList)this.parent).Remove(this);
			}
        }
    }
}