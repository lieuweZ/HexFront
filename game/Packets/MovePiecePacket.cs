using Blok3Game.Engine.JSON;
using Microsoft.Xna.Framework;

namespace Blok3Game.Packets
{
    public class MovePiecePacket : DataPacket
    {
        public string roomId { get; set; }
        public string sourceCell { get; set; }
        public string targetCell { get; set; }
        public string playerName { get; set; }

        public MovePiecePacket()
        {
            EventName = "piece move";
        }

        public MovePiecePacket(string roomId, Vector2 source, Vector2 target, string name)
        {
            EventName = "piece move";
            this.roomId = roomId;
            this.sourceCell = $"{source.X} {source.Y}";
            this.targetCell = $"{target.X} {target.Y}";
            this.playerName = name;
        }

        public Vector2 GetSourceCell()
        {
            string[] coords = sourceCell.Split(' ');
            return new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
        }

        public Vector2 GetTargetCell()
        {
            string[] coords = targetCell.Split(' ');
            return new Vector2(float.Parse(coords[0]), float.Parse(coords[1]));
        }
    }
}