using Blok3Game.Engine.JSON;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.Packets
{
    public class CellInsesrtPacket : DataPacket
    {
        public string tilename { get; set; }
        public int cellcolor { get; set; }
        public bool passable { get; set; }

        public string playerName { get; set; }
        public CellInsesrtPacket() {
            EventName = "cell insert";
        }

        public CellInsesrtPacket(string name, int color, bool passable)
        {
            EventName = "cell insert";
            tilename = name;
            cellcolor = color;
            this.passable = passable;
        }
    }
}
