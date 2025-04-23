using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Blok3Game.Engine.Helpers;
using Blok3Game.GameObjects;
using System.Collections.Generic;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private GameObjectGrid grid;
        private Player player;
        private TextGameObject playerNameText;
        private List<TextGameObject> resourceTexts;

        public GameState() : base()
        {
            grid = new GameObjectGrid(8, 8);
            Add(grid);

            player = new Player("Alice");
            Add(player);

            playerNameText = new TextGameObject("Fonts/SpriteFont", 100);
            playerNameText.Position = new Vector2(10, 10);
            Add(playerNameText);

            resourceTexts = new List<TextGameObject>();
            float yOffset = 40;
            foreach (var res in player.resources)
            {
                TextGameObject resText = new TextGameObject("Fonts/SpriteFont", 100);
                resText.Position = new Vector2(10, yOffset);
                Add(resText);
                resourceTexts.Add(resText);
                yOffset += 25;
            }
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

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            DrawingHelper.FillRectangle(
                new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height),
                spriteBatch,
                new Color(DrawingHelper.GetColorCGA(12))
            );

            base.Draw(gameTime, spriteBatch);
        }
    }
}