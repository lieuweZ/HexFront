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
    public class CellDeletePacket : DataPacket
    {
        public int tileID { get; set; }
        public CellDeletePacket() {
            EventName = "cell delete";
        }
        public CellDeletePacket(int id)
        {
            EventName = "cell delete";
            tileID = id;
        }
    }
}
