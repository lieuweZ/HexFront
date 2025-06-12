using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;
using Blok3Game.Packets;
using System;
using System.Collections.Generic;
using Blok3Game.Engine.SocketIOClient;

namespace Blok3Game.GameObjects
{
    public class Unit : PieceObject
    {
        private static SpriteSheet spriteUnit;
        private static bool assetsLoaded = false;
        public Unit() : base("Images/Sprites/Unit", "unit", 2, 2, "Soldier")
        {
            ResourceCost = 1;
            position = new Vector2(25, 25);
            if (!assetsLoaded)
            {
                LoadAssets();
            }
            sprite = spriteUnit;  // Assign the preloaded sprite
            Origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Optional: center origin
        }

        public static void LoadAssets()
        {
            spriteUnit = new SpriteSheet("Images/Sprites/Unit");
            assetsLoaded = true;
        }

        public Vector2? AttackObjects(Cell myCell, Player player, Vector2 cellPosition)
        {
            var directions = GameState.grid.GetNeighbors((int)cellPosition.X, (int)cellPosition.Y);

            List<Vector2> enemyCell = new();

            foreach (var dir in directions)
            {
                var neighborX = (int)(cellPosition.X + dir.X);
                var neighborY = (int)(cellPosition.Y + dir.Y);

                var neighborCell = GameState.grid.Get(neighborX, neighborY) as Cell;
                if (neighborCell?.Obj is PieceObject piece && piece.OwnerName != GameState.Username)
                {
                    enemyCell.Add(new Vector2(neighborX, neighborY));
                }
            }

            if (enemyCell.Count == 0)
                return null;

            var random = new Random();
            return enemyCell[random.Next(enemyCell.Count)];
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void AtEndTurn(Cell cell, Player player, Vector2 cellPosition)
        {
            Vector2? targetPosition = AttackObjects(cell, player, cellPosition);
            if (targetPosition == null)
            {
                return;
            }
            Console.WriteLine("this is the attack amount" + Attack);
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