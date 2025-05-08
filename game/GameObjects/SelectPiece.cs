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
        public int ID {get; set;}
        private Color currentColor = Color.Red;
        private bool isHovered = false;
        private bool isSelected = false;
        public bool IsHovered => isHovered;
        public bool IsSelected
        {
            get => isSelected;
            set => isSelected = value;
        }
        public SelectPiece(Vector2 position, int width, int height, int id) : base()
        {
            ID = id;
            this.tangle = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            isHovered = tangle.Contains(inputHelper.MousePosition);
        }

        public override void Update(GameTime gameTime)
        {
            if (isSelected)
                currentColor = Color.Green;
            else if (isHovered)
                currentColor = Color.Yellow;
            else
                currentColor = Color.Red;
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            DrawingHelper.DrawRectangle(tangle, spritebatch, currentColor);
        }
    }
}