using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Blok3Game.Engine.Helpers;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private GameObjectGrid grid;
        public GameState() : base()
        {
            grid = new GameObjectGrid(8, 8);
            Add(grid);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawingHelper.FillRectangle(new Rectangle(0,0,GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), spriteBatch, new Color(GetColorCGA(12)));
            base.Draw(gameTime, spriteBatch);
            /*int tileScale = 64;
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    float dispX = ((j & 1) * tileScale / 2F);
                    DrawingHelper.FillHexagon(new Rectangle((int)(0 + i * (tileScale + tileScale / 10) - dispX + tileScale / 10), (int)(0 + j * (tileScale / 1.35)), tileScale, tileScale), spriteBatch, new Color(GetColorEGA((i + j * 16))));
                }   
            }*/

            //DrawingHelper.FillHexagon(new Rectangle(0, 0, 64, 64), spriteBatch, new Color(0xffff5500));
        }

        public uint GetColorCGA(int colorNumber)
        {
            float red = 2F / 3F * (colorNumber & 4) / 4F + 1F / 3F * (colorNumber & 8) / 8F;
            float green = 2F / 3F * (colorNumber & 2) / 2F + 1F / 3F * (colorNumber & 8) / 8F;
            float blue = 2F / 3F * (colorNumber & 1) / 1F + 1F / 3F * (colorNumber & 8) / 8F;

            if (colorNumber == 6)
                green = green * 2 / 3;

            uint color = (uint)(255) << 24 | (uint)(red * 255) << 16 | (uint)(green * 255) << 8 | (uint)(blue * 255);

            return color;
        }


        public uint GetColorEGA(int colorNumber)
        {
            int[] colorbin = new int[6];
            int[] binary = new int[6];

            for (int i = colorNumber; i > 0;i--)
            {
                binary[binary.Length - 1] += 1;
                for (int j = binary.Length - 1; j > 0;j--)
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
