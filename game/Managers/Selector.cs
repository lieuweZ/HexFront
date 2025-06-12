using System;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
    public class Selector : GameObjectList
    {
        private int WIDTH = 40;
        private int HEIGHT = 40;
        private int SPACING = 30;
        private int POSITION_X = 0;
        private InputHelper inputhelper;
        public SelectPiece SelectedPiece { get; private set; }

        public Selector()
        {
            float centerY = GameEnvironment.Screen.Y / 2f;
            float x = GameEnvironment.Screen.X - 70f;
            float y = centerY;

            for (int i = 0; i <= 1; i++)
            {
                y = y + i * (HEIGHT + SPACING);
                Console.WriteLine(y);
                Add(new SelectPiece(new Vector2(x, y), WIDTH, HEIGHT, i));
            }
            Add(new SelectPiece(new Vector2(x, centerY + 2 * (HEIGHT + SPACING)), WIDTH, HEIGHT, 2));
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            inputhelper = inputHelper;
            foreach (SelectPiece piece in Children)
            {
                if (piece.IsHovered && inputHelper.MouseLeftButtonPressed)
                {
                    Select(piece);
                }
            }
        }

        private void Select(SelectPiece piece)
        {
            if (SelectedPiece != null)
            {
                SelectedPiece.IsSelected = false;
            }
            if (piece != SelectedPiece)
            {
                piece.IsSelected = true;
                SelectedPiece = piece;
            }
            else
            {
                piece.IsSelected = false;
                SelectedPiece = null;
            }
        }

        public void Debug()
        {
            if (this.SelectedPiece != null)
            {
                Console.WriteLine("This is the current selected piece" + this.SelectedPiece);
                Console.WriteLine("This is the current selected piece ID" + this.SelectedPiece.ID);
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (SelectPiece piece in Children)
            {
                if (piece.IsHovered)
                {
                    Vector2 textPosition = inputhelper.MousePosition + new Vector2(-50, -50);
                    spriteBatch.DrawString(GameEnvironment.AssetManager.Content.Load<SpriteFont>("Fonts/SpriteFont"), piece.HoverText, textPosition, Color.White);
                }
            }
        }

    }
}