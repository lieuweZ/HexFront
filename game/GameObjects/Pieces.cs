using System;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class PieceObject : SpriteGameObject
	{
        public int ID {get; private set;}
        public string Type {get; private set;}
        public int Attack {get; private set;}
        public int Health {get; private set;}
		public PieceObject(int id, string type, int attack, int health, Vector2 position, string spriteName) : base(spriteName)
		{
            ID = id;
            Type = type;
            Attack = attack;
            Health = health;
            this.Position = position;
		}

        public bool DamageObject (int attack) {
            int current_health = this.Health;
            this.Health -= attack;

            return (this.Health - current_health) < 0;
        }

 //       public void MovePiece ()
    }
}