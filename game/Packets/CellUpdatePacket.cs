using Blok3Game.Engine.JSON;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.Packets
{
    public class CellUpdatePacket : DataPacket
    {
        public string cell { get; set; }
        public string piece { get; set; }
        public string roomId { get; set; }
        public string userName { get; set; }
        public CellUpdatePacket() {
            EventName = "piece update";
        }

        public CellUpdatePacket(string RoomId, Vector2 pos, int type, string name)
        {
            EventName = "piece update";
            roomId = RoomId;
            cell = pos.X + " " + pos.Y;
            this.piece = type + "";
            this.userName = name;
        }
    }
}
