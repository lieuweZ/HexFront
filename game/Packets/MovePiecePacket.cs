using Blok3Game.Engine.JSON;
using Microsoft.Xna.Framework;
using Blok3Game.GameStates;

namespace Blok3Game.Packets
{
    public class MovePiecePacket : DataPacket
    {
        public string roomId { get; set; }
        public string playerName { get; set; }
        public string sourceCell { get; set; }
        public string targetCell { get; set; }

        public MovePiecePacket()
        {
            EventName = "piece move";
        }

public MovePiecePacket(string roomId, Vector2 source, Vector2 target, string playerName)
{
    EventName = "piece move";
    this.roomId = roomId;
    this.playerName = GameState.Username;
    this.sourceCell = $"{source.X} {source.Y}";
    this.targetCell = $"{target.X} {target.Y}";
}    }
}