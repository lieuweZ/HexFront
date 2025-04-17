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

        public static uint GetColorCGA(int colorNumber)
        {
            float red = 2F / 3F * (colorNumber & 4) / 4F + 1F / 3F * (colorNumber & 8) / 8F;
            float green = 2F / 3F * (colorNumber & 2) / 2F + 1F / 3F * (colorNumber & 8) / 8F;
            float blue = 2F / 3F * (colorNumber & 1) / 1F + 1F / 3F * (colorNumber & 8) / 8F;

            if (colorNumber == 6)
                green = green * 2 / 3;

            uint color = (uint)(255) << 24 | (uint)(red * 255) << 16 | (uint)(green * 255) << 8 | (uint)(blue * 255);

            return color;
        }


        public static uint GetColorEGA(int colorNumber)
        {
            int[] colorbin = new int[6];
            int[] binary = new int[6];


            for (int i = colorNumber; i > 0; i--)
            {
                binary[binary.Length - 1] += 1;
                for (int j = binary.Length - 1; j > 0; j--)
                {
                    if (binary[j] > 1)
                    {
                        binary[j] = 0;
                        binary[j - 1]++;
                    }
                }
            }


            int limit = binary.Length;

            if (colorbin.Length < limit)
            {
                limit = colorbin.Length;
            }

            for (int i = 0; i < limit; i++)
            {
                colorbin[colorbin.Length - 1 - i] = binary[limit - 1 - i];
            }


            float red = (colorbin[colorbin.Length - 3] == 1 ? (2F / 3F) : 0) + (colorbin[0] == 1 ? (1F / 3F) : 0);
            float green = (colorbin[colorbin.Length - 2] == 1 ? (2F / 3F) : 0) + (colorbin[1] == 1 ? (1F / 3F) : 0);
            float blue = (colorbin[colorbin.Length - 1] == 1 ? (2F / 3F) : 0) + (colorbin[2] == 1 ? (1F / 3F) : 0);

            uint color = (uint)(255) << 24 | (uint)(red * 255) << 16 | (uint)(green * 255) << 8 | (uint)(blue * 255);

            return color;
        }
    }
}
