namespace Blok3Game.Engine.JSON
{
    public class StartGameData : DataPacket
    {
        public string RoomId { get; set; }

        public string UserId { get; set; }
        
        public StartGameData() : base()
        {
            EventName = "start game";
        }
    }
}
