using System;
using BaseProject;
using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.GameStates
{
    public class MainMenuState : GameObjectList
    {
        private SpriteGameObject TitleText;
        private readonly Button Start;
        private readonly Button Quit;
        private Texture2D mainMenuBackground;
        private Rectangle borders;
        public MainMenuState() : base()
        {
            // mainMenuBackground = GameEnvironment.AssetManager.GetSprite("Images/UI/background_bac");
             //borders = new Rectangle(0, 0, mainMenuBackground.Width, mainMenuBackground.Height);
             TitleText = new SpriteGameObject("Images/UI/Title", 1, "Title");
             TitleText.Position = new Vector2(GameEnvironment.Screen.X / 2 - TitleText.Width / 2, GameEnvironment.Screen.Y * 0.1f);

            GameObjectGrid grid = new GameObjectGrid(9,10);
            grid.Interactible = 2;

            Add(grid);

            Texture2D buttonTexture = GameEnvironment.AssetManager.GetSprite("Images/UI/Button_Big@1x4");
            float scale = 0.2f;
            float buttonWidth = buttonTexture.Width * scale;
            float buttonHeight = buttonTexture.Height * scale;
            float centerX = (GameEnvironment.Screen.X - buttonWidth) / 2;


            // Creates a start button
            Start = new Button(new Vector2(centerX, GameEnvironment.Screen.Y * 0.3f), 0.2f, "Button_Big@1x4")
            {
                Text = "Start",
            };
            Start.Clicked += OnButtonClicked;


            // Creates a quit button
            Quit = new Button(new Vector2(centerX, GameEnvironment.Screen.Y * 0.5f), 0.2f, "Button_Big@1x4")
            {
                Text = "Quit"
            };
            Quit.Clicked += OnButtonClicked;

            Add(TitleText);
            Add(Start);
            Add(Quit);
        }

         public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
         {
            DrawingHelper.FillRectangle(new Rectangle(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), spriteBatch, new Color(DrawingHelper.GetColorCGA(12)));
            base.Draw(gameTime, spriteBatch);
         }

        private void OnButtonClicked(UIElement Element)
        {
            switch (Element)
            {
                case var _ when Element == Start:
                    GameEnvironment.GameStateManager.SwitchTo(GameStateManager.LOBBY_JOIN_OR_CREATE_STATE);
                    break;

                case var _ when Element == Quit:
                    HexFront.self.Quit();
                    break;
                default:
                    Console.WriteLine("Unknown button clicked");
                    break;
            }
        }
    }
}
