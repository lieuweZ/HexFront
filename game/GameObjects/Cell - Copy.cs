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
    public class Cube : GameObject
    {
        public string OwnerName { get; set; }
        public Cube()
        {
            this.position = new Vector2(25, 25); 
        }
        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - this.position.X / 2), (int)(displacement.Y - this.position.Y / 2), 25, 25), spriteBatch, Color.Blue);
        }
    }

    public class Cube2 : GameObject
    {
        public string OwnerName { get; set; }
        public Cube2() 
        {
            this.position = new Vector2(25, 25); 
        }
        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - this.position.X / 2), (int)(displacement.Y - this.position.Y / 2), 25, 25), spriteBatch, Color.Red);
        }
    }

    public class Cube3 : GameObject
    {
        public string OwnerName { get; set; }
        public Cube3() 
        {
            this.position = new Vector2(25, 25); 
        }
        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - this.position.X / 2), (int)(displacement.Y - this.position.Y / 2), 25, 25), spriteBatch, Color.Green);
        }
    }
}
