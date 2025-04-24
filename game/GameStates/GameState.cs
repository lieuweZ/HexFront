using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Blok3Game.Engine.Helpers;
using Blok3Game.Engine.SocketIOClient;
using Blok3Game.GameObjects;
using Blok3Game.Packets;
using System;

namespace Blok3Game.GameStates
{
    public class GameState : GameObjectList
    {
        private PieceSelection selector;
        private GameObjectGrid grid;
        private ObjectManager objectManager;
        public GameState() : base()
        {
            selector = new PieceSelection();
            objectManager = new ObjectManager();
            grid = new GameObjectGrid(8, 8);
            Add(selector);
            Add(grid);
            SocketClient.Instance.SubscribeToDataPacket<CellUpdatePacket>(RecievedData);
        }

        public void RecievedData(object i)
        {
            CellUpdatePacket pack = (CellUpdatePacket)i;
            PieceList pieces = new PieceList();
             string[] pos = pack.cell.Split(" ");



            grid.SetCellPiece(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), pieces.CreateFromId(int.Parse(pack.piece)));


            pack = null;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           // DrawingHelper.FillRectangle(new Rectangle(0,0,GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), spriteBatch, new Color(DrawingHelper.GetColorCGA(12)));
            base.Draw(gameTime, spriteBatch);
        }
    }
}
