using Blok3Game.Engine.JSON;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;

namespace Blok3Game.Packets
{
    public class PieceMovePacket : DataPacket
    {
        public string fromCell { get; set; }
        public string toCell { get; set; }
        public string roomId { get; set; }
        public string playerName { get; set; }

        public PieceMovePacket()
        {
            EventName = "piece move";
        }

        public PieceMovePacket(string RoomId, Vector2 from, Vector2 to)
        {
            EventName = "piece move";
            roomId = RoomId;
            fromCell = from.X + " " + from.Y;
            toCell = to.X + " " + to.Y;
            playerName = GameState.Username;
        }
    }
}