using Blok3Game.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
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
        public int RescoureCost { get; set; }
        public PieceObject(string assestSprite, string type, int attack, int health, string name) : base(assestSprite)
        {
            Type = type;
            Attack = attack;
            Health = health;
            Name = name;


        }

        public void Update(GameTime gameTime)
        {
        }
        public virtual bool TakeDamage(int damageAmount)
        {
            Health -= damageAmount;
            Console.WriteLine("this objectt took damage" + Health + "this is the damage amount:" + damageAmount);
            return Health <= 0;
        }
        public virtual void AtStartTurn(Cell cell, Player player) { }

        public virtual void AtEndTurn(Cell cell, Player player, Vector2 cellPosition) { }

        public virtual void Draw(Vector2 displacement, SpriteBatch spriteBatch)
        {
        }
    }
}