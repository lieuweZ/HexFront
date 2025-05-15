using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameStates;
using Blok3Game.Packets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Blok3Game.GameObjects
{
    public class Player : GameObject
    {
        public static bool myTurn = true;
        public static Timer turnTimer;
        public static double TimePerTurn = 60;

        public string Name { get; set; }
        public List<ResourceType> resources;
        public List<GameObject> hand;
        public GameObject centralBuilding;
        public bool surrendered;

        public Player(string name = "Player 1")
        {
            Name = name;
            resources = ResourceList.resources;
            hand = new List<GameObject>();
            centralBuilding = null;
            surrendered = false;

            turnTimer = new Timer
            {
                Interval = 1000 * TimePerTurn,
                AutoReset = true,
                Enabled = false
            };
            turnTimer.Elapsed += OnTimedEvent;
        }

        private static void endTurn()
        {
            SocketClient.Instance.SendDataPacket(new TurnChangedPacket()
            {
                roomId = SocketClient.Instance.RoomId,
                playerName = GameState.Username
            });
            myTurn = !myTurn;
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"Ending turn at {e.SignalTime}");
            endTurn();
        }

        public override void Update(GameTime gameTime)
        {
            turnTimer.Enabled = myTurn;
            base.Update(gameTime);
        }
    }
}
