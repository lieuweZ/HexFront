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
            Objects[1] = typeof(BuilderCube);
        }

        public GameObject CreateFromId(int id)
        {
            GameObject obj = null;

            Type type = Objects[id];
            if(type != null)
            obj = (GameObject)Activator.CreateInstance(type);

            return obj;
        }


    }
}
