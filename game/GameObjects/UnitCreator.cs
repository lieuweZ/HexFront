using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.Helpers;
using System;

namespace Blok3Game.GameObjects
{
    public class UnitCreator : GameObject
    {
        public string OwnerName { get; set; }
        private int spawnCooldown = 0;
        private const int SPAWN_COOLDOWN_TURNS = 1;
        private const int UNITS_PER_CREATOR = 1; // Limit to 1 unit per creator
        private int unitsSpawned = 0;

        public UnitCreator() : base()
        {
            this.position = new Vector2(25, 25);
        }

        public Unit SpawnUnit()
        {
            if (spawnCooldown > 0 || unitsSpawned >= UNITS_PER_CREATOR)
            {
                return null;
            }

            try
            {
                Unit newUnit = new Unit();
                newUnit.OwnerName = this.OwnerName;
                spawnCooldown = SPAWN_COOLDOWN_TURNS;
                unitsSpawned++;
                return newUnit;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DecrementCooldown()
        {
            if (spawnCooldown > 0)
            {
                spawnCooldown--;
            }
        }

        public void OnTurnStart()
        {
            DecrementCooldown();
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            // Calculate center position for consistent drawing
            int centerX = (int)displacement.X;
            int centerY = (int)displacement.Y;
            
            // Draw base rectangle centered on position
            DrawingHelper.FillRectangle(
                new Rectangle(
                    centerX - 12,  // Half of width (25/2)
                    centerY - 12,  // Half of height (25/2)
                    25,
                    25
                ), 
                spriteBatch, 
                Color.Purple
            );

            // Draw plus symbol centered on the base rectangle
            // Vertical line of plus
            DrawingHelper.FillRectangle(
                new Rectangle(
                    centerX - 2,   // Half of width (4/2)
                    centerY - 8,   // Half of height (16/2)
                    4,
                    16
                ),
                spriteBatch,
                Color.White
            );

            // Horizontal line of plus
            DrawingHelper.FillRectangle(
                new Rectangle(
                    centerX - 8,   // Half of width (16/2)
                    centerY - 2,   // Half of height (4/2)
                    16,
                    4
                ),
                spriteBatch,
                Color.White
            );
        }
    }
}