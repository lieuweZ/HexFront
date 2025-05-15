using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.Packets;
using Blok3Game.Engine.JSON;
using System;
using System.Collections.Generic;
using Blok3Game.Engine.UI;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private Selector selector;
        public static GameObjectGrid grid {get; private set;}
        private Player player;
        private TextGameObject playerNameText;
        private List<TextGameObject> resourceTexts;
        private Button endTurnButton;
        private TextGameObject currentTurnText;
        private string currentTurnPlayerName = "";

        public static string Username = "";

        public GameState() : base()
        {
            selector = new Selector();
            Add(selector);

            grid = new GameObjectGrid(9, 9);
            Add(grid);

            grid.selector = selector;
            
            SocketClient.Instance.SubscribeToDataPacket<CellUpdatePacket>(ReceivedData);
            SocketClient.Instance.SubscribeToDataPacket<TurnChangedPacket>(OnTurnChanged);
            SocketClient.Instance.SubscribeToDataPacket<StartGameData>(StartGame);

            player = new Player("Alice");
            Add(player);

            playerNameText = new TextGameObject("Fonts/SpriteFont", 100);
            playerNameText.Text = "";
            playerNameText.Position = new Vector2(10, 10);
            Add(playerNameText);

            currentTurnText = new TextGameObject("Fonts/SpriteFont", 100);
            currentTurnText.Position = new Vector2(GameEnvironment.Screen.X / 2, 10);
            currentTurnText.Text = "Current Turn: ";
            Add(currentTurnText);


            endTurnButton = new Button(new Vector2(10, 150), 0.05f, "Button_Big@1x4")
            {
                Text = "End Turn",
            };
            endTurnButton.Clicked += OnButtonClicked;
            Add(endTurnButton);
            
            resourceTexts = new List<TextGameObject>();
            float yOffset = 40;
            foreach (var res in player.resources)
            {
                TextGameObject resText = new TextGameObject("Fonts/SpriteFont", 100);
                resText.Position = new Vector2(10, yOffset);
                resText.Text = "";
                Add(resText);
                resourceTexts.Add(resText);
                yOffset += 25;
            }
        }

        public void ReceivedData(object i)
        {
            if (i is CellUpdatePacket piecePacket)
            {
                PieceList pieces = new PieceList();
                string[] pos = piecePacket.cell.Split(" ");
                grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(piecePacket.piece)), piecePacket.playerName);

                piecePacket = null;
            }
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            playerNameText.Text = $"Name: {player.Name}";
            currentTurnText.Text = $"Current Turn: {currentTurnPlayerName}";

            for (int i = 0; i < player.resources.Count; i++)
            {
                var res = player.resources[i];
                resourceTexts[i].Text = $"{res.Name}: {res.Amount}";
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!playerNameText.Text.Equals(Username))
                playerNameText.Text = Username;
            DrawingHelper.FillRectangle(
                new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height),
                spriteBatch,
                new Color(DrawingHelper.GetColorCGA(12))
            );

            base.Draw(gameTime, spriteBatch);
        }

        private void OnTurnChanged(dynamic data)
        {
            string playerName = data.playerName.ToString();
            currentTurnPlayerName = playerName;

            if (playerName == Username)
                Player.myTurn = true;
            else
                Player.myTurn = false;
        }

        private void OnButtonClicked(UIElement element)
        {
            if (element == endTurnButton && Player.myTurn)
            {
                Console.WriteLine("End Turn button clicked.");

                SocketClient.Instance.SendDataPacket(new TurnChangedPacket()
                {
                    roomId = SocketClient.Instance.RoomId,
                    playerName = Username
                });

                Player.myTurn = false;
            }
        }

        private void StartGame(StartGameData data)
        {
            PieceList pieces = new PieceList();

            foreach (string player in data.Players)
            {
                string[] parts = player.Split(':');
                string role = parts[0];
                string PlayerName = parts[1];
                int Column = grid.Columns / 2;
                int Row =  (grid.Rows - 1) * (Int32.Parse(role) - 1);

                grid.SetCellPiece(new Vector2(Column , Row), pieces.CreateFromId(1), PlayerName);
            }   
        }
    }
}