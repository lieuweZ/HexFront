using Blok3Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;

namespace Blok3Game.Engine.GameObjects
{
    public class CentralBuilding : PieceObject
    {
        private static SpriteSheet spriteCentralBuilding;
        private static bool assetsLoaded = false;

        public CentralBuilding() : base("Images/Sprites/Central_Building", "building", 0, 20) // pass null initially, set sprite later
        {
            if (!assetsLoaded)
            {
                LoadAssets();
            }
            sprite = spriteCentralBuilding;  // Assign the preloaded sprite
            Origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Optional: center origin
        }

        public static void LoadAssets()
        {
            spriteCentralBuilding = new SpriteSheet("Images/Sprites/Central_Building");
            assetsLoaded = true;
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