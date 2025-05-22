using Blok3Game.GameObjects;
using Microsoft.Xna.Framework;


namespace Blok3Game.Engine.GameObjects
{
    public class PieceObject : GameObject
    {
        public string OwnerName { get; set; } // Owner of this piece
        public string Type { get; private set; } // Either Building or Unit
        public int Attack { get; private set; }
        public int Health { get; private set; }
        public PieceObject(string type, int attack, int health, Vector2 position) : base()
        {
            Type = type;
            Attack = attack;
            Health = health;
            Position = position;
        }

        public void Update()
        {
        }

        public bool TakeDamage(int attack)
        {
            int current_health = this.Health;
            this.Health -= attack;

            return (this.Health - current_health) < 0;
        }
        public virtual void AtStartTurn(Cell cell, Player player)
        {

        }

        //       public void MovePiece ()
    }
}