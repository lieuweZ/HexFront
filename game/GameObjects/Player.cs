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
        public List<ResourceCollector> Collectors = new List<ResourceCollector>();



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
                Enabled = true
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
            turnTimer.Enabled = false;
        }

        public void BeginTurn()
        {
            Console.WriteLine("begin turn");
            var grid = GameState.grid;
            for (var x = 0; x <= grid.Columns; x++)
            {
                for (var y = 0; y <= grid.Rows; y++)
                {
                    Cell cl = (Cell)grid.Get(x, y);
                    if (cl is Cell && cl.Obj != null)
                    {
                        Console.WriteLine(cl);
                        if (cl.Obj is PieceObject piece && piece.OwnerName == Name)
                        {
                            Console.WriteLine("atstarturn");

                            piece.AtStartTurn();
                        }
                    }
                }
            }
        }

        public void RegisterCollector(ResourceCollector collector)
        {
            Collectors.Add(collector);
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            Console.WriteLine($"Ending turn at {e.SignalTime}");
            endTurn();
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine("Before condition");
            if (myTurn && !turnTimer.Enabled)
            {
                Console.WriteLine("Test");
                BeginTurn();
                turnTimer.Enabled = true;
            }

            base.Update(gameTime);
        }
    }
}
