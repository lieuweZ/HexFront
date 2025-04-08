namespace Blok3Game.Engine.JSON
{
    public class ErrorData : DataPacket
    {
        public string Reason { get; set; }

        public ErrorData() : base()
        {
            EventName = "handle error";
        }
    }
}
