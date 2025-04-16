using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.GameObjects
{
    public class CellType
    {
        public uint Color;
        public bool Passable;
        public bool Damageable;

        public CellType(uint color, bool passable, bool damageable)
        {
            Color = color;
            Passable = passable;
            Damageable = damageable;
        }
    }
}
