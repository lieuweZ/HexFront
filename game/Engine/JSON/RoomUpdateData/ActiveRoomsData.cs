namespace Blok3Game.Engine.JSON
{
    public class ActiveRoomsData : DataPacket
    {		
        public CreateRoomData[] Rooms { get; set; }

        public ActiveRoomsData() : base()
        {
            EventName = "active rooms";
        }
    }
}
