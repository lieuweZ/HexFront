using Blok3Game.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Blok3Game.Engine.GameObjects
{
    public class PieceObject : GameObject
    {
        public string OwnerName { get; set; } // Owner of this piece
        public string Type { get; private set; } // Either Building or Unit
        public int Attack { get; private set; }
        public int Health { get; private set; }
        protected float damageFlashTime = 0f;
        private readonly float maxFlashDuration = 0.2f;
        private readonly Color flashOverlayColor = Color.Red * 0.4f;
        public PieceObject(string type, int attack, int health) : base()
        {
            Type = type;
            Attack = attack;
            Health = health;
        }

        public void Update(GameTime gameTime)
        {
        }
        public bool TakeDamage(int damageAmount)
        {
            damageFlashTime = maxFlashDuration;
            Health -= damageAmount;
            return Health <= 0;
        }
        public virtual void AtStartTurn(Cell cell, Player player) { }

        public virtual void AtEndTurn(Cell cell, Player player) { }

        public virtual void Draw(Vector2 displacement, SpriteBatch spriteBatch)
        {
        }
    }
}