using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blok3Game.GameObjects
{
    public class UniRescourse : ResourceType
    {


        public UniRescourse() : base()
        {
            Id = 0;
            Amount = 0;
            Color = 0;
            Name = "Uni";
        }
    }
}