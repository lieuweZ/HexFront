using Blok3Game.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Blok3Game.GameStates;
namespace Blok3Game.Engine.GameObjects
{
    public class PieceObject : SpriteGameObject
    {
        public string OwnerName { get; set; } // Owner of this piece
        public string Type { get; private set; } // Either Building or Unit
        public int Attack { get; private set; }
        public string Name { get; private set; }
        public int Health { get; protected set; }
        protected float damageFlashTime = 0f;
        private readonly float maxFlashDuration = 0.2f;
        private readonly Color flashOverlayColor = Color.Red * 0.4f;
        public int ResourceCost { get; set; }
        public string HoverText { get; set; }
        public SpriteSheet IconSprite { get; private set; }
        public PieceObject(string assetSprite, string type, int attack, int health, string name) : base(assetSprite)
        {
            Type = type;
            Attack = attack;
            Health = health;
            Name = name;
            IconSprite = new SpriteSheet(assetSprite);
            HoverText = $"HP: {Health} \n ATK: {Attack}";
        }

        public void Update(GameTime gameTime)
        {
        }
        public virtual bool TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            HoverText = $"HP: {Health} \n ATK: {Attack}";
            Console.WriteLine("this objectt took damage" + Health + "this is the damage amount:" + damageAmount);
            return Health <= 0;
        }
        public virtual void AtStartTurn(Cell cell, Player player) { }

        public virtual void AtEndTurn(Cell cell, Player player, Vector2 cellPosition) { }

        public override void Draw(Vector2 displacement, GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(displacement, gameTime, spriteBatch);

            float scale = 1f;
            sprite.Draw(spriteBatch, displacement, Origin, scale, Color.White);

            if (OwnerName == GameState.Username)
            {
                int markerWidth = 24;
                int markerHeight = 4;

                Vector2 markerPos = new Vector2(
                    displacement.X - markerWidth / 2f,
                    displacement.Y + sprite.Height / 2f + 2
                );

                Rectangle markerRect = new Rectangle(
                    (int)markerPos.X,
                    (int)markerPos.Y,
                    markerWidth,
                    markerHeight
                );

                DrawingHelper.FillRectangle(markerRect, spriteBatch, Color.Black);
            }
        }
    }
}