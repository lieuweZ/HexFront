using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Blok3Game.GameObjects
{
    public class Player : GameObject
    {
        private static bool myTurn = true;
        private static Timer turnTimer;
        private List<ResourceType> resources;
        private List<GameObject> hand;
        private GameObject centralBuilding;
        private bool surrendered;

        public Player()
        {
            turnTimer = new Timer();
            // 60 sec
            turnTimer.Interval = 1000 * 60;
            turnTimer.Elapsed += OnTimedEvent;
            turnTimer.AutoReset = true;
            turnTimer.Enabled = false;
        }
        private static void endTurn()
        {
            myTurn = !myTurn;
        }

        public override void Update(GameTime gameTime)
        {
            if (myTurn)
            {
                turnTimer.Enabled = true;
            }
            else
            {
                turnTimer.Enabled = false;
            }
            base.Update(gameTime);
        }
        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Ending turn", e.SignalTime);
            endTurn();
        }
    }
}
