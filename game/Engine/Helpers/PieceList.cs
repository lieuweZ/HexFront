using Blok3Game.Engine.GameObjects;
using Blok3Game.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.Engine.Helpers
{
    public class PieceList
    {
        private Type[] Objects = new Type[32];

        public PieceList()
        {
            Objects[0] = typeof(UnitCreator);
            Objects[1] = typeof(ResourceCollector);
            Objects[2] = typeof(Unit);
            Objects[3] = typeof(CentralBuilding);
        }

        public GameObject CreateFromId(int id)
        {
            GameObject obj = null;

            Type type = Objects[id];
            if (type != null)
                obj = (GameObject)Activator.CreateInstance(type);

            return obj;
        }
        public PieceObject getFromId(int id)
        {
            PieceObject obj = null;

            Type type = Objects[id];
            obj = (PieceObject)Activator.CreateInstance(type);

            return obj;
        }
    }
}
