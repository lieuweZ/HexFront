using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.Packets
{
    public class CellChangePacket : DataPacket
    {
        public int tileID { get; set; }
        public int property { get; set; }
        public string data { get; set; }
        public CellChangePacket() {
            EventName = "cell change";
        }
        public CellChangePacket(int id, int property, string data)
        {
            EventName = "cell change";
            tileID = id;
            this.property = property;
            this.data = data;
        }
    }
}
