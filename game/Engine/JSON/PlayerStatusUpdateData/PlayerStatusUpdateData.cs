namespace Blok3Game.Engine.JSON
{
    public class PlayerStatusUpdateData : DataPacket
    {
        public PlayerData Player { get; set; }
		public string RoomId { get; set; }

		public PlayerStatusUpdateData(string eventName) : base()
		{
			EventName = eventName;
		}
    }
}
