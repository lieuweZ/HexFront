using Blok3Game.Engine.Helpers;
using Blok3Game.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class GameObjectGrid : GameObject
	{
		protected GameObject[,] grid;
		protected int cellWidth = 82, cellHeight = 82;
		public Vector2 MousePos;
		public bool MouseLeftState;
        private int tileScale = 82;

        public GameObjectGrid(int rows, int columns, int layer = 0, string id = "")
			: base(layer, id)
		{
			grid = new GameObject[columns, rows];
			for (int x = 0; x < columns; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					grid[x, y] = null;
				}
			}
		}

        public void Add(GameObject obj, int x, int y)
		{
			grid[x, y] = obj;
			obj.Parent = this;
            float dispX = (((int)y & 1) * tileScale / 2F);

            int PosX = (int)(40 + x * (tileScale + tileScale / 10) - dispX + tileScale / 10);
            int PosY = (int)(40 + y * (tileScale / 1.35));
            obj.Position = new Vector2(PosX, PosY) + obj.Position;
        }

		public GameObject Get(int x, int y)
		{
			if (x >= 0 && x < grid.GetLength(0) && y >= 0 && y < grid.GetLength(1))
			{
				return grid[x, y];
			}
			else
			{
				return null;
			}
		}

		public GameObject[,] Objects
		{
			get
			{
				return grid;
			}
		}

		public Vector2 GetAnchorPosition(GameObject s)
		{
			for (int x = 0; x < Columns; x++)
			{
				for (int y = 0; y < Rows; y++)
				{
					if (grid[x, y] == s)
					{
						return new Vector2(x * cellWidth, y * cellHeight);
					}
				}
			}
			return Vector2.Zero;
		}

		public int Rows
		{
			get { return grid.GetLength(1); }
		}

		public int Columns
		{
			get { return grid.GetLength(0); }
		}

		public int CellWidth
		{
			get { return cellWidth; }
			set { cellWidth = value; }
		}

		public int CellHeight
		{
			get { return cellHeight; }
			set { cellHeight = value; }
		}

		public override void HandleInput(InputHelper inputHelper)
		{
			base.HandleInput(inputHelper);
            MousePos = inputHelper.MousePosition;

            MouseLeftState = inputHelper.MouseLeftButtonPressed;

            foreach (GameObject obj in grid)
			{
                if (obj != null)
                    obj.HandleInput(inputHelper);
			}
		}

		public override void Update(GameTime gameTime)
		{
			foreach (GameObject obj in grid)
			{
                if (obj != null)
                    obj.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            DebugDraw(gameTime, spriteBatch);
            foreach (GameObject obj in grid)
			{
                if (obj != null)
                    obj.Draw(gameTime, spriteBatch);
			}

        }

        public override void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Rows; i++)
			{
				for(int j = 0; j < Columns; j++)
				{
                    float dispX = ((j & 1) * tileScale / 2F);

					int PosX = (int)(40 + i * (tileScale + tileScale / 10) - dispX + tileScale / 10);
					int PosY = (int)(40 + j * (tileScale / 1.35));


                    Color cl = new Color(GetColorEGA((i + j * Rows)));

					if (MousePos.X > PosX && MousePos.X < PosX + tileScale && MousePos.Y > PosY && MousePos.Y < PosY + tileScale)
						if (DrawingHelper.InsideHexagon(new Rectangle(PosX, PosY, tileScale, tileScale), MousePos))
						{
							cl = Color.White;
							if(MouseLeftState)
							{
								GridMouseInput(new Vector2(i, j));
                            }
						}


                    DrawingHelper.FillHexagon(new Rectangle(PosX, PosY, tileScale, tileScale), spriteBatch, cl);
                }
			}
        }

		public void GridMouseInput(Vector2 cell)
		{
            Box box = new Box();
            this.Add(box, (int)cell.X, (int)cell.Y);
        }

        public override void Reset()
		{
			base.Reset();
			foreach (GameObject obj in grid)
			{
				if(obj != null)
				obj.Reset();
			}
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

        public uint GetColorXGA(int colorNumber)
        {
            int[] colorbin = new int[6];
            int[] binary = new int[6];


            for (int i = colorNumber; i > 0; i--)
            {
                binary[binary.Length - 1] += 1;
                for (int j = binary.Length - 1; j > 0; j--)
                {
                    if (binary[j] > 2)
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
			
            float red = (colorbin[colorbin.Length - 3] == 1 ? (2F / 4F) : 0) + (colorbin[0] == 1 ? (1F / 4F) : 0);
            float green = (colorbin[colorbin.Length - 2] == 1 ? (2F / 4F) : 0) + (colorbin[1] == 1 ? (1F / 4F) : 0);
            float blue = (colorbin[colorbin.Length - 1] == 1 ? (2F / 4F) : 0) + (colorbin[2] == 1 ? (1F / 4F) : 0);

            uint color = (uint)(255) << 24 | (uint)(red * 255) << 16 | (uint)(green * 255) << 8 | (uint)(blue * 255);

            return color;
        }
    }
}