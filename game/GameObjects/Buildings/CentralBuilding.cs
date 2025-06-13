using Blok3Game.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameStates;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Packets;

namespace Blok3Game.Engine.GameObjects
{
    public class CentralBuilding : PieceObject
    {
        private static SpriteSheet spriteCentralBuilding;
        private static bool assetsLoaded = false;

        public CentralBuilding() : base("Images/Sprites/Central_Building", "building", 0, 20, "Central Building") // pass null initially, set sprite later
        {
            if (!assetsLoaded)
            {
                LoadAssets();
            }
            sprite = spriteCentralBuilding;  // Assign the preloaded sprite
            Origin = new Vector2(sprite.Width / 2, sprite.Height / 2); // Optional: center origin
        }

        public void PassiveResource(Cell myCell, Player player)
        {
            var target = player.resources.FirstOrDefault(r => r.Id == myCell.Resource.Id);
            if (target != null)
            {
                target.Amount += 1;
            }
        }

        public static void LoadAssets()
        {
            spriteCentralBuilding = new SpriteSheet("Images/Sprites/Central_Building");
            assetsLoaded = true;
        }

        public override void AtStartTurn(Cell cell, Player player)
        {
            PassiveResource(cell, player);
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(displacement, gameTime, spriteBatch);
        }

        public override bool TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            HoverText = $"HP: {Health} \n ATK: {Attack}";
            Console.WriteLine("this objectt took damage" + Health + "this is the damage amount:" + damageAmount);

            return Health <= 0;
        }
    }
}