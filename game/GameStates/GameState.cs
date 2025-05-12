using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.Packets;
using System;
using System.Collections.Generic;
using Blok3Game.Engine.UI;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private GameObjectGrid grid;
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

        public GameState() : base()
        {
            grid = new GameObjectGrid(8, 8);
            Add(grid);
            SocketClient.Instance.SubscribeToDataPacket<CellUpdatePacket>(RecievedData);
            SocketClient.Instance.SubscribeToDataPacket<TurnChangedPacket>(OnTurnChanged);


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
        }

        public void RecievedData(object i)
        {
            if (i is CellUpdatePacket piecePacket)
            {
                PieceList pieces = new PieceList();
                string[] pos = piecePacket.cell.Split(" ");
                grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(piecePacket.piece)));
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!playerNameText.Text.Equals(Username))
                playerNameText.Text = Username;
            DrawingHelper.FillRectangle(
                new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height),
                spriteBatch,
                new Color(DrawingHelper.GetColorCGA(12))
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
            GameEnvironment.AssetManager.AudioManager.PlaySoundEffect("your_turn");
            if (playerName == Username)
            {
                Player.myTurn = true;
                timeRemaining = Player.TimePerTurn;
                elapsedSinceTurnStart = 0;
            }
            else
            {
                Player.myTurn = false;
            }
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

    }
}