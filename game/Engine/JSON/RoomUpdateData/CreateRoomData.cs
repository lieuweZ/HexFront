namespace Blok3Game.Engine.JSON
{
    public class CreateRoomData : DataPacket
    {
        public string RoomId { get; set; }

        public string Author { get; set; }

        public PlayerData[] Players { get; set; }

        public CreateRoomData() : base()
        {
            EventName = "create room";
        }
    }
}
