using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.GameObjects
{
    public class Cubi : GameObject
    {
        public Vector2 size = new Vector2(0, 0);
        public int color;
        public Cubi() 
        {
            size = new Vector2(25, 25); 
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {           
            DrawingHelper.FillRectangle(new Rectangle((int)(this.position.X), (int)(this.position.Y), (int)size.X, (int)size.Y), spriteBatch, new Color(DrawingHelper.GetColorEGA(color)));
        }
    }
}
