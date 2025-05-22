using System;
using System.Collections.Generic;
using Blok3Game.GameStates;


namespace Blok3Game.GameObjects
{
    public class ResourceList
    {
        private static Random rnd = new Random(GameState.Seed);

        public static List<ResourceType> resources = new List<ResourceType>
            {
                new ResourceType ( 0, "Uni",  0,   5),
                new ResourceType (1,   "Wood",   0,   0 ),
                new ResourceType (2,   "Stone",   0,   1 ),
                new ResourceType (3,   "Gold",   0,   2 )
            };


        public ResourceList()
        {
        }

        public ResourceType getRandomResource()
        {
            int r = rnd.Next(resources.Count);
            ResourceType NewResource = new ResourceType(resources[r].Id, resources[r].Name, resources[r].Amount, resources[r].Color);
            return NewResource;
        }
    }

}