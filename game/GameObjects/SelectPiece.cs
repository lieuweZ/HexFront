using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.Engine.GameObjects
{
    public class SelectPiece : GameObject
    {
        private Rectangle tangle;
        public int ID { get; set; }
        private Color currentColor = Color.Red;
        private bool isHovered = false;
        private bool isSelected = false;
        public bool IsHovered => isHovered;
        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }

        public string HoverText { get; set; }
        private SpriteSheet iconSprite;

        public SelectPiece(Vector2 position, int width, int height, int id) : base()
        {
            ID = id;
            this.tangle = new Rectangle((int)position.X, (int)position.Y, width, height);

            PieceList pieceList = new PieceList();
            PieceObject piece = pieceList.getFromId(id);

            if (piece != null)
            {
                string typeLabel = piece.Type == "unit" ? "Units" : "Gold";
                HoverText = $"{piece.Name}\nCost: {piece.ResourceCost}  {typeLabel}";

                iconSprite = piece.IconSprite; // assign icon sprite here
            }
            else
            {
                HoverText = "Unknown Piece";
            }
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            isHovered = tangle.Contains(inputHelper.MousePosition);
        }

        public override void Update(GameTime gameTime)
        {
            if (isSelected)
                currentColor = Color.Black;
            else if (isHovered)
                currentColor = Color.Yellow;
            else
                currentColor = Color.Red;
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            DrawingHelper.DrawRectangle(tangle, spritebatch, currentColor);

            if (iconSprite != null)
            {
                float scale = Math.Min((float)tangle.Width / iconSprite.Width, (float)tangle.Height / iconSprite.Height);

                Vector2 iconPosition = new Vector2(tangle.X, tangle.Y);

                iconSprite.Draw(spritebatch, iconPosition, Vector2.Zero, scale, Color.White);
            }
        }
    }
}