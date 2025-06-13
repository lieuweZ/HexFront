using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;
using Blok3Game.Packets;
using System;
using System.Collections.Generic;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;

namespace Blok3Game.Engine.GameObjects
{
    public class DefensiveBuilding : PieceObject
    {
        private static SpriteSheet spriteDefensiveBuilding;
        private static bool assetsLoaded = false;
        public DefensiveBuilding() : base("Images/Sprites/DefensiveBuilding", "building", 1, 10, "Defense Tower")
        {
            ResourceCost = 3;
            if (!assetsLoaded)
            {
                LoadAssets();
            }
            sprite = spriteDefensiveBuilding;  // Assign the preloaded sprite
            Origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Optional: center origin
        }
        public Vector2? AttackObjects(Vector2 cellPosition)
        {
            var directions = GameState.grid.GetNeighbors((int)cellPosition.X, (int)cellPosition.Y);

            List<Vector2> enemyCell = new();

            foreach (var dir in directions)
            {
                var neighborX = (int)(cellPosition.X + dir.X);
                var neighborY = (int)(cellPosition.Y + dir.Y);

                var neighborCell = GameState.grid.Get(neighborX, neighborY) as Cell;
                if (neighborCell?.Obj is Unit unit && unit.OwnerName != GameState.Username)
                {
                    enemyCell.Add(new Vector2(neighborX,neighborY));
                }
            }

            if (enemyCell.Count == 0)
                return null;

            var random = new Random();
            return enemyCell[random.Next(enemyCell.Count)];
        }

        public static void LoadAssets()
        {
            spriteDefensiveBuilding = new SpriteSheet("Images/Sprites/DefensiveBuilding");
            assetsLoaded = true;
        }
        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void AtEndTurn(Cell cell, Player player, Vector2 cellPosition)
        {
            Vector2? targetPosition = AttackObjects(cellPosition);
            if (targetPosition == null)
            {
                return;
            }
            DamagePacket packet = new DamagePacket(
                new Vector2(targetPosition.Value.X, targetPosition.Value.Y),
                Attack
            );
            SocketClient.Instance.SendDataPacket(packet);
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