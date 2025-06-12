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

        private static SpriteSheet spriteUnitCreator;
        private static bool assetsLoaded = false;

        public UnitCreator() : base("Images/Sprites/Barrack", "building", 0, 5, "Barrack")
        {
            RescoureCost = 3;
            this.position = new Vector2(25, 25);
            if (!assetsLoaded)
            {
                LoadAssets();
            }
            sprite = spriteUnitCreator;  // Assign the preloaded sprite
            Origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Optional: center origin
        }

        public static void LoadAssets()
        {
            spriteUnitCreator = new SpriteSheet("Images/Sprites/Barrack");
            assetsLoaded = true;
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
            if (sprite != null)
            {
                float scale = 1f; // adjust if you want to scale your sprite
                sprite.Draw(spriteBatch, displacement, Origin, scale, Color.White);
            }
        }
    }
}