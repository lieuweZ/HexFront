using Blok3Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;

namespace Blok3Game.Engine.GameObjects
{
    public class CentralBuilding : PieceObject
    {

        public CentralBuilding() : base("building", 0, 20)
        {
        }
        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - 12), (int)(displacement.Y - 12), 25, 25), spriteBatch, Color.White);
        }
    }
}