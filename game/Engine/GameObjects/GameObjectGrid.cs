using BaseProject;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.Packets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Blok3Game.Engine.GameObjects
{
	public class GameObjectGrid : GameObject
	{
		protected GameObject[,] grid;
		protected int cellWidth = 82, cellHeight = 82;
		public Vector2 MousePos;
		public bool MouseLeftState;
		private int prevCellScale = HexFront.self.CellScale;
        private int curCellScale = HexFront.self.CellScale;
        public int Interactible = 0;
		private int ignoreNumber = 9;
        private Random rand = new Random();

        public GameObjectGrid(int rows, int columns, int layer = 0, string id = "")
			: base(layer, id)
		{
			grid = new GameObject[columns, rows];
			for (int x = 0; x < columns; x++)
			{
				for (int y = 0; y < rows; y++)
				{
					grid[x, y] = null;
                    Add(new Cell(),x,y);
                }
			}
		}

        public void Add(GameObject obj, int x, int y)
		{
            Vector2 pos = GetHexagonPos(x, y);
            int PosX = (int)pos.X;
            int PosY = (int)pos.Y;

            grid[x, y] = obj;
			obj.Parent = this;
            obj.Position = new Vector2(PosX, PosY) + this.position;
        }

		public void Resize()
		{
            int tileScale = curCellScale;

			for (int x = 0;x < Columns; x++)
			{
				for(int y = 0;y < Rows; y++)
				{
                    Vector2 pos = GetHexagonPos(x, y);
                    int PosX = (int)pos.X;
                    int PosY = (int)pos.Y;

                    GameObject gm = this.Get(x, y);

                    gm.Position = new Vector2(PosX, PosY) + this.position;
                }
			}
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
			for (int x = 0; x < Rows; x++)
			{
				for (int y = 0; y < Columns; y++)
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
			if (Interactible == 0 || Interactible == 2)
			{
			MousePos = inputHelper.MousePosition;

			MouseLeftState = inputHelper.MouseLeftButtonPressed;
			}

            foreach (GameObject obj in grid)
			{
                if (obj != null)
                    obj.HandleInput(inputHelper);
			}
		}

		public override void Update(GameTime gameTime)
		{
			if (prevCellScale != HexFront.self.CellScale)
			{
                curCellScale = HexFront.self.CellScale;
                this.Resize();
				prevCellScale = curCellScale;
            }



            foreach (GameObject obj in grid)
			{
                if (obj != null)
                    obj.Update(gameTime);
			}
		}

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
            foreach (GameObject obj in grid)
			{
                if (obj != null)
                    obj.Draw(gameTime, spriteBatch);
			}
            DebugDraw(gameTime, spriteBatch);
        }

		public Vector2 GetHexagonPos(int i, int j)
		{
            int tileScale = curCellScale;
            float dispX = ((j & 1) * tileScale / 1.825F);

            int PosX = (int)(((tileScale * 1.5F) * (Rows == ignoreNumber ? 0.25 : 1)) + i * (tileScale + tileScale / 10) - dispX + tileScale / 2.95);
            int PosY = (int)(((tileScale / 5) * (Rows == ignoreNumber ? 0 : 1)) + j * (tileScale / 1.35));

			return new Vector2(PosX, PosY);
        }

        public override void DebugDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int tileScale = curCellScale;
            for (int i = 0; i < Columns; i++)
			{
				for(int j = 0; j < Rows; j++)
				{
					Vector2 pos = GetHexagonPos(i, j);
					int PosX = (int)pos.X;
					int PosY = (int)pos.Y;


                    Color cl = new Color(DrawingHelper.GetColorCGA((i + j * Rows)));

					if (Interactible == 0 || Interactible == 2)
					{
						if (MousePos.X > PosX && MousePos.X < PosX + tileScale && MousePos.Y > PosY && MousePos.Y < PosY + tileScale)
						if (DrawingHelper.InsideHexagon(new Rectangle(PosX, PosY, tileScale, tileScale), MousePos))
						{
							cl = Color.White;
							if (MouseLeftState)
							{
								GridMouseInput(new Vector2(i, j));
							}
							GameObject ce = this.Get(i, j);
                            if (Interactible == 0)
                            ce.DebugDraw(gameTime, spriteBatch);
						}
					}
					else
					if(Interactible == 1)
					{
                        Cell ce = (Cell)this.Get(i, j);
                        if (rand.Next(1952) >= 1922 && ce.GlowTime == 0)
						{
                            ce.GlowTime = 50;
                        } else
						{
							if(ce.GlowTime > 0)
							ce.GlowTime--;
						}

						if(ce.GlowTime > 0)
						{
                            DrawingHelper.FillHexagon(new Rectangle(PosX, PosY, tileScale, tileScale), spriteBatch, cl);
                        }
					}


                    //DrawingHelper.FillHexagon(new Rectangle(PosX, PosY, tileScale, tileScale), spriteBatch, cl);
                }
			}
        }

		public void GridMouseInput(Vector2 cell)
		{
            
            if (Interactible == 2)
			{
				MinigameInput(cell);
            } else
			{
				PlacePiece(cell, 1);
            }
        }

		public void PlacePiece(Vector2 pos, int id)
		{
			CellUpdatePacket pack = new CellUpdatePacket(SocketClient.Instance.RoomId, pos, id);

            if (id == 1)
			{
                SocketClient.Instance.SendDataPacket(pack);
            } else
			{
                SocketClient.Instance.SendDataPacket(new CellUpdatePacket(SocketClient.Instance.RoomId, pos, 0));
            }
            
        }

		public void SetCellPiece(Vector2 cell, GameObject box)
		{
            Cell cl = (Cell)(this.Get((int)cell.X, (int)cell.Y));
            cl.SetObject(box);
        }

		public void MinigameInput(Vector2 cell)
		{
            Cube box = new Cube();
            Cell cl = (Cell)(this.Get((int)cell.X, (int)cell.Y));
            cl.SetObject(box);
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    cl = (Cell)(this.Get(i, j));
                    if (cl.Obj == null)
                    {
                        return;
                    }
                }
            }
            for (int i = 0; i < Columns; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    cl = (Cell)(this.Get(i, j));
                    cl.ClearObject();
                }
            }
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
    }
}