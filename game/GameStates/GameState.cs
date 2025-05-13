using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.Packets;
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
        private GameObjectGrid grid;
        private Player player;
        private TextGameObject playerNameText;
        private List<TextGameObject> resourceTexts;
        public static string Username = "";

        private List<string> messages = new List<string>();
        private TextGameObject chatText;
        private Rectangle chatBorder;
        private TextInput playerNameInput;

        public GameState() : base()
        {
            grid = new GameObjectGrid(8, 8);

            playerNameInput = new TextInput(new Vector2(0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 70), 0.1F);
            Add(playerNameInput);

            Add(grid);
            SocketClient.Instance.SubscribeToDataPacket<CellUpdatePacket>(RecievedData);
            SocketClient.Instance.SubscribeToDataPacket<ChatMessagePacket>(RecievedMsg);

            player = new Player("Alice");
            Add(player);

            playerNameText = new TextGameObject("Fonts/SpriteFont", 100);
            playerNameText.Text = "";
            playerNameText.Position = new Vector2(10, 10);
            Add(playerNameText);

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

        public void RecievedData(object i)
        {
            CellUpdatePacket pack = (CellUpdatePacket)i;
            PieceList pieces = new PieceList();
            string[] pos = pack.cell.Split(" ");



            grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(pack.piece)));


            pack = null;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            playerNameText.Text = $"Name: {player.Name}";

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
            if(!playerNameText.Text.Equals(GameState.Username))
            playerNameText.Text = GameState.Username;
            DrawingHelper.FillRectangle(
                new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height),
                spriteBatch,
                new Color(DrawingHelper.GetColorCGA(9))
            );

            base.Draw(gameTime, spriteBatch);
        }
    }
}