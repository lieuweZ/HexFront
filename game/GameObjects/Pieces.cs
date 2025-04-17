using System;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class PieceObject : SpriteGameObject
	{
        public string Type {get; private set;} // Either Building or Unit
        public int Attack {get; private set;}
        public int Health {get; private set;}
		public PieceObject(string type, int attack, int health, Vector2 position, string spriteName) : base(spriteName)
		{
            Type = type;
            Attack = attack;
            Health = health;
            this.Position = position;
		}

        public bool TakeDamage (int attack) {
            int current_health = this.Health;
            this.Health -= attack;

            return (this.Health - current_health) < 0;
        }

 //       public void MovePiece ()
    }
}