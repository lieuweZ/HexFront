using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.GameObjects
{
    public class Player : GameObject
    {
        private bool myTurn;
        private List<int> resources;
        private List<int> hand;
        private GameObject centralBuilding;

        public Player()
        {
        }


    }
}
