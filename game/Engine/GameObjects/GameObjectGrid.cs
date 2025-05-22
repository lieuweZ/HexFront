using BaseProject;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.JSON;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.Packets;
using Blok3Game.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Blok3Game.Engine.GameObjects
{
	public class GameObjectGrid : GameObject
	{
		protected GameObject[,] grid;
		private Vector2? selectedCellCoord = null;
		private Cell selectedCell;
		protected int cellWidth = 82, cellHeight = 82;
		public Vector2 MousePos;
		public bool MouseLeftState;
		private int prevCellScale = HexFront.self.CellScale;
        private int curCellScale = HexFront.self.CellScale;
        public int Interactible = 0;
		private int ignoreNumber = 9;
		public Selector selector {get; set;}
        private Random rand = new Random();

        public GameObjectGrid(int rows, int columns, int layer = 0, string id = "")
			: base(layer, id)
		{
			grid = new GameObject[columns, rows];
			selector = new Selector();
			
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
			for (int j = 0; j < Rows; j++)
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
    int x = (int)cell.X;
    int y = (int)cell.Y;

    // Early return if selector is null
    if (selector == null)
    {
        Console.WriteLine("Warning: Selector is not initialized");
        return;
    }

    if (!Player.myTurn)
    {
        return;
    }

    Cell clickedCell = Get(x, y) as Cell;
    if (clickedCell == null)
    {
        return;
    }

    // If no cell is selected and a unit is clicked
    if (selectedCellCoord == null && clickedCell.Obj is Unit selectedUnit)
    {
        if (selectedUnit.OwnerName == GameState.Username && Player.myTurn)
        {
            selectedCellCoord = cell;
            clickedCell.GlowTime = 100;
        }
        return;
    }

    // Safe access to SelectedPiece with null check
    if (selector.SelectedPiece?.GetType() == typeof(UnitCreator))
    {
        if (clickedCell.Obj == null)
        {
            UnitCreator creator = new UnitCreator();
            creator.OwnerName = GameState.Username;
            clickedCell.SetObject(creator);
            
            CellUpdatePacket packet = new CellUpdatePacket(
                SocketClient.Instance.RoomId,
                cell,
                0,
                GameState.Username
            );
            SocketClient.Instance.SendDataPacket(packet);
        }
        return;
    }

    // Add null check for selector
    if (selector == null || selector.SelectedPiece == null)
    {
        return;
    }

    if (x < 0 || x >= Columns || y < 0 || y >= Rows)
        return;

    // If no cell is selected
    if (selectedCellCoord == null)
    {
        if (clickedCell.Obj != null)
        {
            if (clickedCell.Obj is UnitCreator creator && creator.OwnerName == GameState.Username)
            {
                Vector2[] neighbors = GetNeighbors(x, y);
                if (neighbors != null)
                {
                    foreach (Vector2 dir in neighbors)
                    {
                        int nx = x + (int)dir.X;
                        int ny = y + (int)dir.Y;
                        
                        Cell neighborCell = Get(nx, ny) as Cell;
                        if (neighborCell?.Obj == null)
                        {
                            Unit newUnit = creator.SpawnUnit();
                            if (newUnit != null)
                            {
                                newUnit.OwnerName = GameState.Username;
                                neighborCell.SetObject(newUnit);
                                break;
                            }
                        }
                    }
                }
            }
            else if (clickedCell.Obj is Unit movableUnit && movableUnit.OwnerName == GameState.Username)
            {
                selectedCellCoord = cell;
                clickedCell.GlowTime = 100;
            }
        }
        else
        {
            // Handle piece placement
            int? ID = selector.SelectedPiece?.ID;
            if (ID.HasValue && CanPlaceAt(cell))
            {
                PlacePiece(cell, ID.Value);
            }
        }
    }
    // If a cell is already selected
    else
    {
        Vector2 selected = selectedCellCoord.Value;
        Cell sourceCell = Get((int)selected.X, (int)selected.Y) as Cell;

        // Double check ownership before allowing movement
        if (sourceCell?.Obj is Unit unit && unit.OwnerName != GameState.Username)
        {
            selectedCellCoord = null;
            return;
        }

        bool isNeighbor = GetNeighbors((int)selected.X, (int)selected.Y)
            .Any(dir => x == selected.X + dir.X && y == selected.Y + dir.Y);

        if (isNeighbor && clickedCell?.Obj == null && Player.myTurn)
        {
            // Move the object only if it's a Unit and belongs to current player
            if (sourceCell?.Obj is Unit unitToMove && unitToMove.OwnerName == GameState.Username)
            {
                clickedCell.SetObject(sourceCell.Obj);
                sourceCell.ClearObject();
                MovePiece(selected, cell);
            }
            selectedCellCoord = null;
        }
        else
        {
            selectedCellCoord = null;
        }
    }
}

public void MovePiece(Vector2 source, Vector2 target)
{


    MovePiecePacket packet = new MovePiecePacket(
        SocketClient.Instance.RoomId,
        source,
        target,
        GameState.Username
    );
    
    SocketClient.Instance.SendDataPacket(packet);

}

public void HandleRemoteMove(Vector2 source, Vector2 target)
{
    
    Cell sourceCell = Get((int)source.X, (int)source.Y) as Cell;
    Cell targetCell = Get((int)target.X, (int)target.Y) as Cell;

    if (sourceCell?.Obj != null && targetCell != null)
    {
        GameObject piece = sourceCell.Obj;
        sourceCell.ClearObject();
        targetCell.SetObject(piece);
    }
}

		public void PlacePiece(Vector2 pos, int id)
		{

			CellUpdatePacket pack = new CellUpdatePacket(
				SocketClient.Instance.RoomId, 
				pos, 
				id, 
				GameState.Username
			);
			SocketClient.Instance.SendDataPacket(pack);
        }

		public void SetCellPiece(Vector2 cell, GameObject box, string playerName)
		{
			Cell cl = (Cell)(this.Get((int)cell.X, (int)cell.Y));

			if (cl.Obj != null)
			{
				return;
			}

			if (box is Unit unit)
			{
				unit.OwnerName = playerName;
			}
			else if (box is Cube2 cube2)
			{
				cube2.OwnerName = playerName;
			}
			else if (box is Cube3 cube3)
			{
				cube3.OwnerName = playerName;
			}

			cl.SetObject(box);
		}


		private bool CanPlaceAt(Vector2 pos)
		{
			int x = (int)pos.X;
			int y = (int)pos.Y;

			Cell current = Get(x, y) as Cell;

			if (current == null || current.Obj != null)
			{
				return false;
			}

			if (CheckIfAvailable())
			{
				return true;
			}

			Vector2[] selectedDirections = GetNeighbors(x,y);

			foreach (Vector2 dir in selectedDirections)
			{
				int nx = x + (int)dir.X;
				int ny = y + (int)dir.Y;

				if (nx == x && ny == y)
				{
					continue;
				}

				Cell neighbor = Get(nx, ny) as Cell;

				if (neighbor != null && neighbor.Obj != null)
				{
					string ownerName = GameState.Username;
					var obj = neighbor.Obj;
					bool isOwned =
						(obj is Cube cube && cube.OwnerName == ownerName) ||
						(obj is Cube2 cube2 && cube2.OwnerName == ownerName) ||
						(obj is Cube3 cube3 && cube3.OwnerName == ownerName);

					if (isOwned)
					{
						return true; 
					}
				}
			}
			return false;
		}


		private bool CheckIfAvailable()
		{
			string ownerName = GameState.Username;

			foreach (GameObject obj in grid)
			{
				if (obj is Cell cell && cell.Obj != null)
				{
					var placedObj = cell.Obj;

					if ((placedObj is Cube cube && cube.OwnerName == ownerName) ||
						(placedObj is Cube2 cube2 && cube2.OwnerName == ownerName) ||
						(placedObj is Cube3 cube3 && cube3.OwnerName == ownerName))
					{
						return false;
					}
				}
			}
			return true;
		}


		// Use this function to return a Vector2 array of all possible neighbors (including non-existent ones) for the given x and y values.
		private Vector2[] GetNeighbors(int x, int y)
		{
			Vector2[] directionsEven = new Vector2[]
			{
				new Vector2(-1,  0), // West
				new Vector2(+1,  0), // East
				new Vector2( 0, +1), // South-West
				new Vector2(+1, +1), // South-East
				new Vector2( 0, -1), // North-West
				new Vector2(+1, -1), // North-East
			};

			Vector2[] directionsOdd = new Vector2[]
			{
				new Vector2(-1,  0), // West
				new Vector2(+1,  0), // East
				new Vector2(-1, +1), // South-West
				new Vector2( 0, +1), // South-East
				new Vector2(-1, -1), // North-West
				new Vector2( 0, -1), // North-East
			};

			return (y % 2 == 0) ? directionsEven : directionsOdd;
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

        public void OnTurnStart()
        {
            int spawnsThisTurn = 0;
            const int MAX_SPAWNS_PER_TURN = 3;
            var unitCreators = new List<(UnitCreator creator, int x, int y)>();
            
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Cell cell = Get(x, y) as Cell;
                    if (cell?.Obj is UnitCreator creator && creator.OwnerName == GameState.Username)
                    {
                        creator.OnTurnStart();
                        unitCreators.Add((creator, x, y));
                    }
                }
            }

            foreach (var (creator, x, y) in unitCreators)
            {
                if (spawnsThisTurn >= MAX_SPAWNS_PER_TURN) break;

                var neighbors = GetNeighbors(x, y)
                    .Select(dir => new { 
                        nx = x + (int)dir.X, 
                        ny = y + (int)dir.Y 
                    })
                    .Where(n => {
                        if (n.nx < 0 || n.nx >= Columns || n.ny < 0 || n.ny >= Rows)
                            return false;
                        Cell neighborCell = Get(n.nx, n.ny) as Cell;
                        return neighborCell?.Obj == null;
                    })
                    .ToList();

                if (neighbors.Any())
                {
                    var rnd = new Random();
                    var selected = neighbors[rnd.Next(neighbors.Count)];
                    
                    Unit newUnit = creator.SpawnUnit();
                    if (newUnit != null)
                    {
                        Cell targetCell = Get(selected.nx, selected.ny) as Cell;
                        newUnit.OwnerName = GameState.Username;
                        targetCell.SetObject(newUnit);
                        spawnsThisTurn++;

                        CellUpdatePacket packet = new CellUpdatePacket(
                            SocketClient.Instance.RoomId,
                            new Vector2(selected.nx, selected.ny),
                            1,
                            GameState.Username
                        );
                        SocketClient.Instance.SendDataPacket(packet);
                    }
                }
            }
        }

        public void OnTurnEnd()
        {
            for (int x = 0; x < Columns; x++)
            {
                for (int y = 0; y < Rows; y++)
                {
                    Cell cell = Get(x, y) as Cell;
                    if (cell?.Obj is Unit attacker && attacker.OwnerName == GameState.Username)
                    {
                        var neighbors = GetNeighbors(x, y);
                        foreach (Vector2 dir in neighbors)
                        {
                            int nx = x + (int)dir.X;
                            int ny = y + (int)dir.Y;
                            
                            Cell neighborCell = Get(nx, ny) as Cell;
                            if (neighborCell?.Obj is Unit target && target.OwnerName != GameState.Username)
                            {
                                if (target.TakeDamage(attacker.Damage))
                                {
                                    neighborCell.ClearObject();
                                }
                            }
                        }
                    }
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