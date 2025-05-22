using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;

namespace Blok3Game.GameObjects
{
    public class Unit : GameObject
    {
        public string OwnerName { get; set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }

        public Unit() : base()
        {
            this.position = new Vector2(25, 25);
            Health = 100;
            Damage = 20;
        }

        public bool TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            return Health <= 0;
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Head
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - 5),
                    (int)(displacement.Y - 20),
                    10, 10
                ),
                spriteBatch,
                Color.Blue
            );

            // Body
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - 6),
                    (int)(displacement.Y - 10),
                    12, 15
                ),
                spriteBatch,
                Color.Blue
            );

            // Left arm
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - 12),
                    (int)(displacement.Y - 8),
                    6, 4
                ),
                spriteBatch,
                Color.DarkBlue
            );

            // Right arm (holding weapon)
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X + 6),
                    (int)(displacement.Y - 8),
                    12, 4
                ),
                spriteBatch,
                Color.DarkBlue
            );

            // Legs
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - 6),
                    (int)(displacement.Y + 5),
                    5, 10
                ),
                spriteBatch,
                Color.DarkBlue
            );
            
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X + 1),
                    (int)(displacement.Y + 5),
                    5, 10
                ),
                spriteBatch,
                Color.DarkBlue
            );
        }
    }
}