using Blok3Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;

namespace Blok3Game.Engine.GameObjects
{
    public class ResourceCollector : PieceObject
    {

        public int ResourceCost { get; set; }
        public ResourceCollector() : base("building", 0, 5)
        {
            ResourceCost = 1;
        }

        public void CollectResource(Cell myCell, Player player)
        {
            if (myCell.Resource != null && myCell.Resource.Amount > 0)
            {
                var target = player.resources.FirstOrDefault(r => r.Id == myCell.Resource.Id);
                if (target != null)
                {
                    target.Amount += 1;
                    myCell.Resource.Amount -= 1;
                //    Console.WriteLine($"{GameState.Username} collected 1 {myCell.Resource.Name}. Remaining in cell: {myCell.Resource.Amount}");
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        
        public override void AtStartTurn(Cell cell, Player player)
        {
            CollectResource(cell, player);
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - 12), (int)(displacement.Y - 12), 25, 25), spriteBatch, Color.Orange);
            base.Draw(displacement, spriteBatch);
        }
    }

}