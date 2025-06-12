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
        public int ResourceCost { get; set; }
        public Unit() : base("unit", 2, 2)
        {
            ResourceCost = 1;
            position = new Vector2(25, 25);
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
                    enemyCell.Add(new Vector2(neighborX,neighborY));
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

            base.Draw(displacement, spriteBatch);
        }
    }
}