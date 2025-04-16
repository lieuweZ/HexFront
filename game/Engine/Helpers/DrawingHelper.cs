using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Blok3Game.Engine.Helpers
{
	public class DrawingHelper
	{
		protected static Texture2D pixel;

		public static void Initialize(GraphicsDevice graphics)
		{
			pixel = new Texture2D(graphics, 1, 1);
			pixel.SetData(new[] { Color.White });
		}

		public static void DrawRectangle(Rectangle r, SpriteBatch spriteBatch, Color col)
		{
			int bw = 2; // Border width

			spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, bw, r.Height), col); // Left
			spriteBatch.Draw(pixel, new Rectangle(r.Right, r.Top, bw, r.Height), col); // Right
			spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, bw), col); // Top
			spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Bottom, r.Width, bw), col); // Bottom
		}

        public static void FillRectangle(Rectangle r, SpriteBatch spriteBatch, Color col)
        {
            int bw = 2; // Border width

            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, r.Height), col); // Left
        }

        public static void FillHexagon(Rectangle r, SpriteBatch spriteBatch, Color col)
        {
            int bw = 2; // Border width

            //spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2, r.Y, bw, r.Height), col); // Top

            float disp = ((r.Width / 2F) / (r.Height / 3F)) / 5;


            for (int i = 0; i < r.Width / 2; i++)
            {
                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 + i, (int)(r.Y + i * (1.0 - disp)), bw, (int)(r.Height / 2.7)), col); // Top
                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 - i, (int)(r.Y + i * (1.0 - disp)), bw, (int)(r.Height / 2.7)), col); // Top


                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 + i, (int)(r.Y + r.Height / 1.5 - i * (1.0 - disp)), bw, (int)(r.Height / 2.7)), col); // Top
                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 - i, (int)(r.Y + r.Height / 1.5 - i * (1.0 - disp)), bw, (int)(r.Height / 2.7)), col); // Top
            }

            spriteBatch.Draw(pixel, new Rectangle(r.X, (int)((r.Y + r.Height / 3)), r.Width, r.Height / 3), col); // Top
        }

        public static bool InsideHexagon(Rectangle r, Vector2 pos)
        {
            int bw = 2; // Border width

            //spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2, r.Y, bw, r.Height), col); // Top

            float disp = ((r.Width / 2F) / (r.Height / 3F)) / 5;



            int rects = 1;
            for (int i = 0; i < r.Width * 2; i++)
            {
                rects++;
            }

            Rectangle[] rectangles = new Rectangle[rects];

                for (int i = 0; i < r.Width / 2; i++)
            {
                rectangles[i] = new Rectangle(r.X + r.Width / 2 + i, (int)(r.Y + i * (1.0 - disp)), bw, (int)(r.Height / 2.7)); // Top
                rectangles[i + r.Width / 2] = new Rectangle(r.X + r.Width / 2 - i, (int)(r.Y + i * (1.0 - disp)), bw, (int)(r.Height / 2.7)); // Top


                rectangles[i + (r.Width / 2) * 2] = new Rectangle(r.X + r.Width / 2 + i, (int)(r.Y + r.Height / 1.5 - i * (1.0 - disp)), bw, (int)(r.Height / 2.7)); // Top
                rectangles[i + (r.Width / 2) * 3] = new Rectangle(r.X + r.Width / 2 - i, (int)(r.Y + r.Height / 1.5 - i * (1.0 - disp)), bw, (int)(r.Height / 2.7)); // Top
            }

            rectangles[rects - 1] = new Rectangle(r.X, (int)((r.Y + r.Height / 3)), r.Width, r.Height / 3); // Top

            for(int j = 0;j < rectangles.Length;j++)
            {
                Rectangle re = rectangles[j];
                if (re.X < pos.X && re.X + re.Width > pos.X && re.Y < pos.Y && re.Y + re.Height > pos.Y)
                {
                    return true;
                }
            }

            return false;
        }

        public static void DrawHexagon(Rectangle r, SpriteBatch spriteBatch, Color col)
        {
            int bw = 2; // Border width
            spriteBatch.Draw(pixel, new Rectangle(r.X, r.Y + r.Height / 3, bw, r.Height / 3), col); // Top
            spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width, r.Y + r.Height / 3, bw, r.Height / 3), col); // Top

            spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2, r.Y, bw, bw), col); // Top
            spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2, r.Y + r.Height, bw, bw), col); // Top

            float disp = ((r.Width / 2F) / (r.Height / 3F)) / 5;


            for (int i = 0; i < r.Width / 2; i++)
            {
                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 + i, (int)(r.Y + i * (1.0 - disp)), bw, bw), col); // Top
                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 - i, (int)(r.Y + i * (1.0 - disp)), bw, bw), col); // Top


                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 + i, (int)(r.Y + r.Height - i * (1.0 - disp)), bw, bw), col); // Top
                spriteBatch.Draw(pixel, new Rectangle(r.X + r.Width / 2 - i, (int)(r.Y + r.Height - i * (1.0 - disp)), bw, bw), col); // Top
            }
        }
    }
}
