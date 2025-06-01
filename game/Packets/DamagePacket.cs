using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Blok3Game.Packets
{
    public class DamagePacket : DataPacket
    {
        public string roomId { get; set; }
        public string targetPosition { get; set; } 
        public int attackDamage { get; set; }
        public DamagePacket() : base()
        {
            EventName = "object to be damaged found";
        }
        public DamagePacket(Vector2? pos, int attack)
        {
            EventName = "object to be damaged found";
            roomId = SocketClient.Instance.RoomId;
            targetPosition = pos.Value.X + " " + pos.Value.Y;
            attackDamage = attack;
        }
    }
}
