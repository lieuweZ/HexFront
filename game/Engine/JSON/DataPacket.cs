using System.Text.Json;

namespace Blok3Game.Engine.JSON
{
    //abstract class because we don't want to be able to create an instance of this class, only of the derived classes that have specific data.
    public partial class DataPacket
    {
        public string EventName { get; protected set; }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static T FromJson<T>(string json) where T : DataPacket, new()
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
