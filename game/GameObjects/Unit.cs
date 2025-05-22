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
            Health = 100;  // Starting health
            Damage = 20;   // Base damage
        }

        public bool TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            return Health <= 0;  // Returns true if unit dies
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.FillRectangle(
                new Rectangle(
                    (int)(displacement.X - this.position.X / 2), 
                    (int)(displacement.Y - this.position.Y / 2), 
                    25, 25
                ), 
                spriteBatch, 
                Color.Blue
            );
        }
    }
}