using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.GameObjects
{
    public class CellEffect
    {
        public int effectId;
        public string effectname;
        public int damage;
        public int stunTurns;

        public int startDelay;
        public int endDelay;

        public CellEffect(int id, string name, int damage, int stunlength)
        {
            effectId = id;
            this.effectname = name;
            this.damage = damage;
            this.stunTurns = stunlength;
        }

        public CellEffect(int id, string name, int damage, int stunlength, int start, int end)
        {
            effectId = id;
            this.effectname = name;
            this.damage = damage;
            this.stunTurns = stunlength;
            this.startDelay = start;
            this.endDelay = end;
    }

        public string tostring()
        {
            return effectId + "=" + effectname + "=" + damage + "=" + stunTurns + "=" + startDelay + "=" + endDelay;
        }

        public CellEffect fromString(string str)
        {
            string[] data = str.Split("=");
            return new CellEffect(int.Parse(data[0]), data[1], int.Parse(data[2]), int.Parse(data[3]), int.Parse(data[4]), int.Parse(data[5]));
        }
    }
}
