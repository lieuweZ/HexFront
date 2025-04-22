using Blok3Game.GameObjects;
using Microsoft.Xna.Framework;

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

    }
}