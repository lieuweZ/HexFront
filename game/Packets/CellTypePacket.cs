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
    public class CellTypePacket : DataPacket
    {
        public string roomId { get; set; }
        public string username { get; set; }
        public int tileID { get; set; }
        public string name { get; set; }
        public int color { get; set; }
        public bool passable { get; set; }
        public CellTypePacket() {
            EventName = "cell type";
            roomId = SocketClient.Instance.RoomId;
            username = GameState.Username;
        }
        public CellTypePacket(int id)
        {
            EventName = "cell type";
            tileID = id;
            roomId = SocketClient.Instance.RoomId;
            username = GameState.Username;
        }

        public CellTypePacket(string event1,int id)
        {
            EventName = event1;
            tileID = id;
            roomId = SocketClient.Instance.RoomId;
            username = GameState.Username;
        }
    }
}
