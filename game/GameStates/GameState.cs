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
using System.Net.Mail;
using System.Runtime;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private Selector selector;
        public static GameObjectGrid grid { get; private set; }
        private Player player;
        private TextGameObject playerNameText;
        private List<TextGameObject> resourceTexts;
        private Button endTurnButton;
        private TextGameObject currentTurnText;
        private TextGameObject timerText;
        private double timeRemaining = Player.TimePerTurn;
        private double elapsedSinceTurnStart = 0;
        private Texture2D pieTimerTexture;
        private Vector2 pieTimerPosition;
        private float pieTimerRotation;
        private string currentTurnPlayerName = "";
        public static string Username = "";
        private List<string> messages = new List<string>();
        private TextGameObject chatText;
        private Rectangle chatBorder;
        private TextInput playerNameInput;

        public static int Seed { get; set; }
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

            SocketClient.Instance.SubscribeToDataPacket<CellEffectPacket>(RecievedCellEffectData);
            SocketClient.Instance.SubscribeToDataPacket<ChatMessagePacket>(RecievedMsg);
            SocketClient.Instance.SubscribeToDataPacket<MovePiecePacket>(OnMovePieceReceived);
            SocketClient.Instance.SubscribeToDataPacket<TurnChangedPacket>(OnTurnChanged);
            SocketClient.Instance.SubscribeToDataPacket<StartGameData>(StartGame);
            SocketClient.Instance.SubscribeToDataPacket<DamagePacket>(DamageCell);


            player = new Player("player");
            Add(player);

            playerNameText = new TextGameObject("Fonts/SpriteFont", 100);
            playerNameText.Text = "";
            playerNameText.Position = new Vector2(10, 10);
            Add(playerNameText);

            currentTurnText = new TextGameObject("Fonts/SpriteFont", 100);
            currentTurnText.Position = new Vector2(GameEnvironment.Screen.X / 2, 10);
            currentTurnText.Text = "Current Turn: ";
            Add(currentTurnText);

            timerText = new TextGameObject("Fonts/SpriteFont", 100);
            timerText.Position = new Vector2(GameEnvironment.Screen.X - 135, 10);
            timerText.Text = "";
            Add(timerText);

            pieTimerTexture = GameEnvironment.AssetManager.GetSprite("Images/UI/fill");
            pieTimerPosition = new Vector2(GameEnvironment.Screen.X - 100, 100);

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
            TextGameObject unitText = new TextGameObject("Fonts/SpriteFont", 100);
            unitText.Position = new Vector2(10, yOffset);
            unitText.Text = "";
            Add(unitText);
            resourceTexts.Add(unitText);
            yOffset += 100;

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
            if (messages.Count < int.MaxValue)
                messages.Add(msg);
            recalculcateMessage();
        }

        public void recalculcateMessage()
        {

            int message = messages.Count;
            int msg_limit = 8;
            string[] msg = messages.ToArray();
            string chat = "";

            if (message > msg_limit)
            {
                message = msg_limit;
            }

            for (int i = 0; i < message; i++)
            {
                if (i > 0)
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
                        cell.cell = new CellType(pack.tileID, DrawingHelper.GetColorEGA(4), pack.passable, false);
                    }
                }
            }

            grid.updated = true;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // UpdateGridObjects(gameTime);

            playerNameText.Text = $"Name: {player.Name}";
            currentTurnText.Text = $"Current Turn: {currentTurnPlayerName}";

            for (int i = 0; i < player.resources.Count; i++)
            {
                var res = player.resources[i];
                resourceTexts[i].Text = $"{res.Name}: {res.Amount}";
            }

            resourceTexts[1].Text = $"Units: {player.UnitsAvailable}";
            if (Player.myTurn)
            {
                elapsedSinceTurnStart += gameTime.ElapsedGameTime.TotalSeconds;
                double displayTime = Math.Max(0, timeRemaining - elapsedSinceTurnStart);
                timerText.Text = $"Time Left: {Math.Ceiling(displayTime)}s";
                pieTimerRotation = (float)((1 - displayTime / timeRemaining) * MathHelper.TwoPi);
            }
            else
            {
                timerText.Text = "Time Left: 60s";
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (playerNameInput != null && playerNameInput.Text != null)
                if (inputHelper.IsKeyDown(Keys.Enter) && playerNameInput.Text.Length > 0)
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

            spriteBatch.Draw(
                pieTimerTexture,
                pieTimerPosition,
                null,
                Color.White,
                pieTimerRotation,
                new Vector2(pieTimerTexture.Width / 2f, pieTimerTexture.Height / 2f),
                1f,
                SpriteEffects.None,
                0f
            );
            base.Draw(gameTime, spriteBatch);
        }

        private void OnTurnChanged(dynamic data)
        {
            string playerName = data.playerName.ToString();
            currentTurnPlayerName = playerName;
            //Console.WriteLine($"Turn changed to: {playerName}");
            GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("your_turn");
            if (playerName == Username)
            {
                Console.WriteLine("Begin turn is working");
                Player.myTurn = true;
                grid.OnTurnStart();  // Make sure this line is present
                timeRemaining = Player.TimePerTurn;
                elapsedSinceTurnStart = 0;
                player.BeginTurn();
            }
            else
            {
                player.EndTurn();
                grid.OnTurnEnd();
                Player.myTurn = false;
            }
        }

        private void OnButtonClicked(UIElement element)
        {
            if (element == endTurnButton && Player.myTurn)
            {
                //Console.WriteLine("End Turn button clicked.");

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
            grid.UpdateCells(data.RoomSeed);

            foreach (string player in data.Players)
            {
                string[] parts = player.Split(':');
                string role = parts[0];
                string PlayerName = parts[1];
                int Column = grid.Columns / 2;
                int Row = (grid.Rows - 1) * (int.Parse(role) - 1);
                grid.SetCellPiece(new Vector2(Column, Row), pieces.CreateFromId(4), PlayerName);
            }
        }
        private void OnMovePieceReceived(object data)
        {
            if (data is MovePiecePacket movePacket)
            {
                // Handle ALL moves, not just remote ones
                Vector2 source = movePacket.GetSourceCell();
                Vector2 target = movePacket.GetTargetCell();
                grid.HandleRemoteMove(source, target);
            }
        }

        private void DamageCell(DamagePacket data)
        {
            string[] position = data.targetPosition.Split();
            int x = int.Parse(position[0]);
            int y = int.Parse(position[1]);
            Cell cl = (Cell)grid.Get(x, y);
            if (cl.Obj is PieceObject piece)
            {
                if (piece.TakeDamage(data.attackDamage))
                {
                    cl.ClearObject();
                }
            }
        }
    }
}