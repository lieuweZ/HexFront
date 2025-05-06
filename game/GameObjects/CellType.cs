using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.GameObjects
{
    public class CellType
    {
        private int tileID;
        private uint Color;
        private bool Passable;
        private bool Damageable;

        public CellType(int id, uint color, bool passable, bool damageable)
        {
            this.tileID = id;
            Color = color;
            Passable = passable;
            Damageable = damageable;
        }
        public int getTileId()
        {
            return tileID;
        }

        public uint GetColor()
        {
            return Color;
        }

        public bool GetPassable()
        {
            return Passable;
        }

        public bool CanCauseDamage()
        {
            return Damageable;
        }
    }
}
