using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blok3Game.Engine.Helpers;
using BaseProject;

namespace Blok3Game.GameObjects
{
    public class Cell : GameObject
    {
        public GameObject Obj;
        public ResourceType Resource;
        public CellType cell;
        public Cell()
        {
            cell = new CellType(DrawingHelper.GetColorEGA(34), true, false) ;
        }

        public override void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int tileScale = HexFront.self.CellScale;
            DrawingHelper.DrawHexagon(new Rectangle((int)this.position.X, (int)this.position.Y, tileScale, tileScale), spriteBatch, new Color(DrawingHelper.GetColorEGA(15)));
        }

        public void SetObject(GameObject obj)
        { 
            Obj = obj;
            obj.Position = obj.Position;
        }

        public override void Update(GameTime gameTime)
        {
            if (Obj != null)
            {
                Obj.Update(gameTime);
            }
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int tileScale = HexFront.self.CellScale;
            DrawingHelper.FillHexagon(new Rectangle((int)this.position.X, (int)this.position.Y, tileScale, tileScale), spriteBatch, new Color(cell.Color));

            Vector2 displacementhalf = new Vector2(tileScale / 2, tileScale / 2);

            if (Resource != null)
            {
                Resource.Draw(this.position + displacementhalf, gameTime, spriteBatch);
            }
            if (Obj != null)
            {
                Obj.Draw(this.position + displacementhalf, gameTime, spriteBatch);
            }

            
            
            
            //DrawingHelper.FillRectangle(new Rectangle((int)this.position.X, (int)this.position.Y, 25, 25), spriteBatch, Color.Blue);
        }
    }
}
