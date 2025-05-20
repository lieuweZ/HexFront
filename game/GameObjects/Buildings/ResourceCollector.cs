using Blok3Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.Engine.GameObjects
{
    public class ResourceCollector : PieceObject
    {
        private Player player;
        private Cell myCell;

        public string OwnerName { get; set; }

        public ResourceCollector() : base("building", 0, 5, Vector2.Zero)
        {
        }

        public void CollectResource()
        {
            if (myCell.Resource != null && myCell.Resource.Amount > 0)
            {

                var target = player.resources.FirstOrDefault(r => r.Id == myCell.Resource.Id);
                if (target != null)
                {
                    target.Amount += 1;
                    myCell.Resource.Amount -= 1;
                    Console.WriteLine($"{player.Name} collected 1 {myCell.Resource.Name}. Remaining in cell: {myCell.Resource.Amount}");
                }
            }
        }

        public void AtStartTurn()
        {
            Console.WriteLine("dsfs");
            CollectResource();
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - this.position.X / 2), (int)(displacement.Y - this.position.Y / 2), 25, 25), spriteBatch, Color.Orange);
        }

    }

}