using System;
using Microsoft.Xna.Framework;

namespace Blok3Game.Engine.GameObjects
{
	public class Animation : SpriteSheet
	{
		protected float frameTime;
		protected bool isLooping;
		protected float time;

		public Animation(string assetname, bool isLooping, float frameTime = 0.1f) : base(assetname)
		{
			this.frameTime = frameTime;
			this.isLooping = isLooping;
		}

		public void Play(int startSheetIndex)
		{
			sheetIndex = startSheetIndex;
			time = 0.0f;
		}

		public void Update(GameTime gameTime)
		{
			time += (float)gameTime.ElapsedGameTime.TotalSeconds;

			while (time > frameTime)
			{
				time -= frameTime;
				if (IsLooping) // go to the next frame, or loop around
					SheetIndex = (SheetIndex + 1) % NumberOfSheetElements;
				else // go to the next frame if it exists
					SheetIndex = Math.Min(SheetIndex + 1, NumberOfSheetElements - 1);
			}
		}

		public float FrameTime
		{
			get { return frameTime; }
		}

		public bool IsLooping
		{
			get { return isLooping; }
		}

		public int CountFrames
		{
			get { return NumberOfSheetElements; }
		}

		public bool AnimationEnded
		{
			get { return !isLooping && sheetIndex >= NumberOfSheetElements - 1; }
		}
	}
}
