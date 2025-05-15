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
    public class CellUpdatePacket : DataPacket
    {
        public string cell { get; set; }
        public string piece { get; set; }
        public string roomId { get; set; }

        public string playerName { get; set; }
        public CellUpdatePacket() {
            EventName = "cell update";
        }

        public CellUpdatePacket(string RoomId, Vector2 pos, int type)
        {
            EventName = "cell update";
            roomId = RoomId;
            cell = pos.X + " " + pos.Y;
            this.piece = type + "";
            playerName = GameState.Username;
        }
    }
}
