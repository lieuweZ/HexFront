namespace Blok3Game.Engine.JSON
{
    public class SessionData : DataPacket
    {
        public string SessionId { get; set; }

        public string UserId { get; set; }

        public string RoomId { get; set; }

        public SessionData() : base()
        {
            EventName = "session established";
        }

        public SessionData(string sessionId, string userId) : base()
        {
            this.SessionId = sessionId;
            this.UserId = userId;
            EventName = "session established";
        }
    }
}
