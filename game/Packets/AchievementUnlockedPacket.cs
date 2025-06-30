using Blok3Game.Engine.JSON;

namespace Blok3Game.Packets
{
    public class AchievementUnlockedPacket : DataPacket
    {
        public string playerName { get; set; }
        public AchievementData achievement { get; set; }
        
        public AchievementUnlockedPacket()
        {
            EventName = "achievement unlocked";
        }
    }

    public class AchievementData
    {
        public int id { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int points { get; set; }
        public string category { get; set; }
    }
}
