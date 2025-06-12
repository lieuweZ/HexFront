using Blok3Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;
using Blok3Game.Engine.GameObjects;

namespace Blok3Game.GameObjects
{
    public class UnitCreator : PieceObject
    {
        private int spawnCooldown = 0;
        private const int SPAWN_COOLDOWN_TURNS = 1;
        private const int UNITS_PER_CREATOR = 1; // Limit to 1 unit per creator
        private int unitsSpawned = 0;

        public UnitCreator() : base("building", 0, 5)
        {
            RescoureCost = 3;
            this.position = new Vector2(25, 25);
        }

        public void CollectUnit(Cell myCell, Player player)
        {
            player.UnitsAvailable += 1;
            //    Console.WriteLine($"{GameState.Username} collected 1 {myCell.Resource.Name}. Remaining in cell: {myCell.Resource.Amount}");
        }
        public override void AtStartTurn(Cell cell, Player player)
        {
            CollectUnit(cell, player);
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

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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

            base.Draw(displacement, spriteBatch);
        }
    }
}