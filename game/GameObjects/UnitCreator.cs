using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.GameObjects
{
    public class UnitCreator : GameObject
    {
        public string OwnerName { get; set; }

        public UnitCreator() : base()
        {
            this.position = new Vector2(25, 25);
        }

        public Unit SpawnUnit()
        {
            Unit newUnit = new Unit();
            newUnit.OwnerName = this.OwnerName;
            return newUnit;  // Add this return statement
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            // Draw base rectangle
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - this.position.X / 2), 
                    (int)(displacement.Y - this.position.Y / 2), 
                    25, 25
                ), 
                spriteBatch, 
                Color.Purple
            );

            // Draw plus symbol (keeping original design)
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - 2),
                    (int)(displacement.Y - 8),
                    4, 16
                ),
                spriteBatch,
                Color.White
            );

            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - 8),
                    (int)(displacement.Y - 2),
                    16, 4
                ),
                spriteBatch,
                Color.White
            );
        }
    }
}