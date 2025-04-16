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
    public class Cell : GameObject
    {
        public GameObject Obj;
        public ResourceType Resource;
        public CellType cell;
        private int tileScale = 82;
        public Cell()
        {
            cell = new CellType(DrawingHelper.GetColorEGA(34), true, false) ;
        }

        public override void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.DrawHexagon(new Rectangle((int)this.position.X, (int)this.position.Y, tileScale, tileScale), spriteBatch, new Color(DrawingHelper.GetColorEGA(15)));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.FillHexagon(new Rectangle((int)this.position.X, (int)this.position.Y, tileScale, tileScale), spriteBatch, new Color(cell.Color));


            
            if(Resource != null)
            {
                Resource.Draw(gameTime, spriteBatch);
            }
            if (Obj != null)
            {
                Obj.Draw(gameTime, spriteBatch);
            }

            


            //DrawingHelper.FillRectangle(new Rectangle((int)this.position.X, (int)this.position.Y, 25, 25), spriteBatch, Color.Blue);
        }
    }
}
