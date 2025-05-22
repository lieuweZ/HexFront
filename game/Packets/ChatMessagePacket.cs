using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using SocketIO.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.Packets
{
    public class ChatMessagePacket : DataPacket
    {
        public string sender { get; set; }
        public string message { get; set; }
        public string roomId { get; set; }
        public ChatMessagePacket() {
            EventName = "chat msg";
        }

        public ChatMessagePacket(string msg)
        {
            EventName = "chat msg";
            this.roomId = SocketClient.Instance.RoomId;
            this.message = msg;
            this.sender = GameState.Username;
        }
    }
}
