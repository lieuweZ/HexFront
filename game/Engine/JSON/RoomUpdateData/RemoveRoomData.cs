namespace Blok3Game.Engine.JSON
{
    public class RemoveRoomData : DataPacket
    {
        public string RoomId { get; set; }

        public RemoveRoomData() : base()
        {
            EventName = "remove room";
        }
    }
}
