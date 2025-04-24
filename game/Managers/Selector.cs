using System;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
    public class PieceSelection : GameObjectList
    {
        private int WIDTH = 40;
        private int HEIGHT = 40;
        private int SPACING = 30;
        private int POSITION_Y = 0;
        public SelectPiece SelectedPiece {get; set;}

        public PieceSelection()
        {
            float centerX = GameEnvironment.Screen.X / 2f - WIDTH / 2f;
            float y = POSITION_Y;

            for (int i = -1; i <= 1; i++)
            {
                switch(int i)
                {
                    case -1:
                        Add(new SelectPiece(new Vector2(x, y), WIDTH, HEIGHT,
                        {
                            new 
                        }));
                }
                float x = centerX + i * (WIDTH + SPACING);
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

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
            } else
            {
                piece.IsSelected = false;
                SelectedPiece = null;
            }
        }
    }
}