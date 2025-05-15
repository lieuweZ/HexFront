using System;
using System.Collections.Generic;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class ObjectManager
    {
        private PieceObject[] GamePieces;

        public ObjectManager()
        {
            
        }

        public void UpdateObjects()
        {
            foreach(PieceObject piece in GamePieces)
            {
                piece.Update();
            }
        }
    }
}