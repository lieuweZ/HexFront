using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Blok3Game.Packets
{
    public class GameOverPacket : DataPacket
    {
        public string roomId { get; set; }
        public GameOverPacket() : base()
        {
            EventName = "gameOver";
            roomId = SocketClient.Instance.RoomId;
        }
    }
}
