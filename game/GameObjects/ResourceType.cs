using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Blok3Game.GameObjects
{
    public class ResourceType
    {
        public int Id;
        public int Amount;
        public int Color;
        public string Name;

        public ResourceType(int id, string name, int amount, int color)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Color = color;
        }

        public void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
