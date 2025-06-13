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
        private static SpriteSheet spriteResourceCollector;
        private static bool assetsLoaded = false;
        public ResourceCollector() : base("Images/Sprites/Colector", "building", 0, 5, "Resource Collector")
        {
            ResourceCost = 1;
            if (!assetsLoaded)
            {
                LoadAssets();
            }
            sprite = spriteResourceCollector;  // Assign the preloaded sprite
            Origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Optional: center origin
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

        public static void LoadAssets()
        {
            spriteResourceCollector = new SpriteSheet("Images/Sprites/Colector");
            assetsLoaded = true;
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
            base.Draw(displacement, gameTime, spriteBatch);
        }
    }

}