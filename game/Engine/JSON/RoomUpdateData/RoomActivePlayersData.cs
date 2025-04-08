namespace Blok3Game.Engine.JSON
{
    public class RoomActivePlayersData : DataPacket
    {
        public string RoomId { get; set; }

        public string Author { get; set; }

        //contains the data of all players in a room, including the client that sent the request.
		public PlayerData[] Players { get; set; }

        public RoomActivePlayersData() : base()
        {
            EventName = "room active players";
        }
    }
}
