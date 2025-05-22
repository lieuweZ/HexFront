using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.Packets
{
    public class CellEffectPacket : DataPacket
    {
        public string roomId { get; set; }
        public string username { get; set; }
        public int cell { get; set; }
        public string celleffect { get; set; }
        public CellEffectPacket() {
            EventName = "cell effects";
            celleffect = new CellEffect(0, "-", 0, 0, 0, 0).tostring();
            roomId = SocketClient.Instance.RoomId;
            username = GameState.Username;
        }

        public CellEffectPacket(int type)
        {
            EventName = "cell effects";
            cell = type;
            celleffect = new CellEffect(0,"-",0,0,0,0).tostring();
            roomId = SocketClient.Instance.RoomId;
            username = GameState.Username;
        }
    }
}
