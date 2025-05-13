using Blok3Game.Engine.JSON;

namespace Blok3Game.Packets
{
    public class TurnChangedPacket : DataPacket
    {
        public string roomId { get; set; }
        public string playerName { get; set; }

        public TurnChangedPacket()
        {
            EventName = "turn changed";

        }
        
        
    }



}
