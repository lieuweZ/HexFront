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
        public ResourceCollector() : base("building", 0, 5, Vector2.Zero, "")
        {
            Position = position;
        }

        public void CollectResource()
        {
            ResourceType resource = myCell.Resource;
            myCell.Resource.Amount -= 1;
            player.resources.Add(resource);
        }

        public void atStartTurn()
        {
            CollectResource();
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {           
            DrawingHelper.FillRectangle(new Rectangle((int)(displacement.X - this.position.X / 2), (int)(displacement.Y - this.position.Y / 2), 25, 25), spriteBatch, Color.Black);
        }
    }
}