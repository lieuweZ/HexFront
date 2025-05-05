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

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private Selector selector;
        private GameObjectGrid grid;
        private Player player;
        private TextGameObject playerNameText;
        private List<TextGameObject> resourceTexts;
        public static string Username = "";

        public GameState() : base()
        {
            selector = new Selector();
            Add(selector);

            grid = new GameObjectGrid(8, 8);
            Add(grid);
            grid.selector = selector;


            SocketClient.Instance.SubscribeToDataPacket<CellUpdatePacket>(ReceivedData);

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
        }

        public void DebugLogAllCubes()
		{
			for (int x = 0; x < grid.Columns; x++)
			{
				for (int y = 0; y < grid.Rows; y++)
				{
					Cell cell = grid.Get(x, y) as Cell;
					if (cell?.Obj != null)
					{
						string owner = "Unknown";
						if (cell.Obj is Cube cube) owner = cube.OwnerName;
						else if (cell.Obj is Cube2 cube2) owner = cube2.OwnerName;
						else if (cell.Obj is Cube3 cube3) owner = cube3.OwnerName;

						Console.WriteLine($"Cube at ({x},{y}) is owned by: {owner}");
					}
				}
			}
		}

        public void ReceivedData(object i)
        {
            DebugLogAllCubes();
            CellUpdatePacket pack = (CellUpdatePacket)i;
            PieceList pieces = new PieceList();
            string[] pos = pack.cell.Split(" ");    


            grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(pack.piece)), pack.userName);

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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if(!playerNameText.Text.Equals(GameState.Username))
            playerNameText.Text = GameState.Username;
            DrawingHelper.FillRectangle(
                new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height),
                spriteBatch,
                new Color(DrawingHelper.GetColorCGA(12))
            );

            base.Draw(gameTime, spriteBatch);
        }
    }
}