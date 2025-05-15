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
using static System.Net.Mime.MediaTypeNames;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework.Input;
using System.Transactions;

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

        private List<string> messages = new List<string>();
        private TextGameObject chatText;
        private Rectangle chatBorder;
        private TextInput playerNameInput;

        public GameState() : base()
        {
            selector = new Selector();
            Add(selector);

            grid = new GameObjectGrid(9, 9);

            playerNameInput = new TextInput(new Vector2(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 70), 0.1F);
            Add(playerNameInput);

            Add(grid);

            grid.selector = selector;
            
            SocketClient.Instance.SubscribeToDataPacket<CellUpdatePacket>(ReceivedData);
            SocketClient.Instance.SubscribeToDataPacket<CellTypePacket>(RecievedCellData);

            SocketClient.Instance.SubscribeToDataPacket<CellEffectPacket> (RecievedCellEffectData);
            SocketClient.Instance.SubscribeToDataPacket<ChatMessagePacket>(RecievedMsg);
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

            chatText = new TextGameObject("Fonts/SpriteFont", 100);
            chatText.Position = new Vector2(10, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 250);
            chatText.Text = "";
            Add(chatText);
        }

        public void RecievedMsg(object i)
        {
            ChatMessagePacket pack = (ChatMessagePacket)i;
            PieceList pieces = new PieceList();
            string msg = pack.sender + " : " + pack.message;
            if(messages.Count < int.MaxValue)
            messages.Add(msg);
            recalculcateMessage();
        }

        public void recalculcateMessage()
        {

            int message = messages.Count;
            int msg_limit = 8;
            string[] msg = messages.ToArray();
            string chat = "";

            if(message > msg_limit)
            {
                message = msg_limit;
            }

            for(int i = 0;i < message; i++) {
                if(i > 0)
                {
                    chat += "\n";
                }
                chat += msg[((messages.Count - (message)) + i)];
            }


            chatText.Text = chat;
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

        public void RecievedCellData(object i)
        {
            CellTypePacket pack = (CellTypePacket)i;
            PieceList pieces = new PieceList();
            //string[] pos = pack.cell.Split(" ");

            UpdateTile(pack);

            //grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(pack.piece)));
        }

        public void RecievedCellEffectData(object i)
        {
            CellEffectPacket pack = (CellEffectPacket)i;
            PieceList pieces = new PieceList();
            //string[] pos = pack.cell.Split(" ");

            //grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(pack.piece)));
        }

        public void UpdateTile(CellTypePacket pack)
        {
            for (int x = 0; x < grid.Columns; x++)
            {
                for (int y = 0; y < grid.Rows; y++)
                {
                    Cell cell = (Cell)grid.Get(x, y);
                    if (pack.tileID == cell.cell.getTileId())
                    {
                        cell.cell = new CellType(pack.tileID, DrawingHelper.GetColorEGA(pack.color), pack.passable, false);
                    }
                }
            }

            grid.updated = true;
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

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if(playerNameInput != null && playerNameInput.Text != null)
            if (inputHelper.IsKeyDown(Keys.Enter)  && playerNameInput.Text.Length > 0)
            {
                ChatMessagePacket pack = new ChatMessagePacket(playerNameInput.Text);
                SocketClient.Instance.SendDataPacket(pack);
                playerNameInput.Clear();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!playerNameText.Text.Equals(Username))
                playerNameText.Text = Username;
            DrawingHelper.FillRectangle(
                new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height),
                spriteBatch,
                new Color(DrawingHelper.GetColorCGA(9))
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