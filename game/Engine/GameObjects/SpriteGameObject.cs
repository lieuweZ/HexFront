using System;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.Engine.GameObjects
{
	public class SpriteGameObject : GameObject
	{
		protected Color shade = Color.White;
		protected SpriteSheet sprite;
		protected Vector2 origin;
		protected float scale = 1f;
		public bool PerPixelCollisionDetection = true;

		private bool bDebug = false;
		private SpriteSheet sprBoundingbox;


		public SpriteGameObject(string assetName, int layer = 0, string id = "", int sheetIndex = 0, bool bDebug = false)
			: base(layer, id)
		{
			if (assetName != "")
			{
				sprite = new SpriteSheet(assetName, sheetIndex);
			}
			else
			{
				throw new Exception("No asset name provided for sprite game object.");
			}

			// set debug values such as custom image and wether its on or off
			// NOTE: current image might not exit set your own
			this.bDebug = bDebug;
			sprBoundingbox = new SpriteSheet(assetName, sheetIndex); 
		}    

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			if (!visible || sprite == null)
			{
				return;
			}
			sprite.Draw(spriteBatch, GlobalPosition, Origin, scale, shade);

			// Draw collision indication
			if (bDebug)
			{
				DrawBoundingBox(spriteBatch);
			}
		}

		public void DrawBoundingBox(SpriteBatch spriteBatch)
		{
			if (!visible || sprBoundingbox == null)
			{
				return;
			}

			// Draw custom debug shape
			// draw corners
			sprBoundingbox.Draw(spriteBatch, new Vector2 (BoundingBox.Left, BoundingBox.Top), sprBoundingbox.Center, 0.4f, shade);
			sprBoundingbox.Draw(spriteBatch, new Vector2 (BoundingBox.Right, BoundingBox.Top), sprBoundingbox.Center, 0.4f, shade);
			sprBoundingbox.Draw(spriteBatch, new Vector2 (BoundingBox.Left, BoundingBox.Bottom), sprBoundingbox.Center, 0.4f, shade);
			sprBoundingbox.Draw(spriteBatch, new Vector2 (BoundingBox.Right, BoundingBox.Bottom), sprBoundingbox.Center, 0.4f, shade);

			// draw center point
			sprBoundingbox.Draw(spriteBatch, Position, sprBoundingbox.Center, 0.4f, shade);
		}

		public SpriteSheet Sprite
		{
			get { return sprite; }
		}

		public Vector2 Center
		{
			get { return new Vector2(Width, Height) / 2; }
		}

		public int Width
		{
			get
			{
				return (int)(sprite.Width * Scale);
			}
		}

		public int Height
		{
			get
			{
				return (int)(sprite.Height * Scale);
			}
		}

		/// <summary>
		/// Returns / sets the scale of the sprite.
		/// </summary>
		public float Scale 
		{
			get { return scale; }
			set { scale = value;  }
		}

		/// <summary>
		/// Set the shade the sprite will be drawn in.
		/// </summary>
		public Color Shade {
			get { return shade; }
			set { shade = value; }
		}

		public bool Mirror
		{
			get { return sprite.Mirror; }
			set { sprite.Mirror = value; }
		}

		public Vector2 Origin
		{
			get { return origin; }
			set { origin = value; }
		}
		
		public override Rectangle BoundingBox
		{
			get
			{
				// Scale the origin to match the drawn sprite
				int left = (int)Math.Floor(GlobalPosition.X - origin.X * Scale);
				int top = (int)Math.Floor(GlobalPosition.Y - origin.Y * Scale);
				// Use Ceiling to ensure the box fully encloses the sprite
				int width = (int)Math.Ceiling(sprite.Width * Scale);
				int height = (int)Math.Ceiling(sprite.Height * Scale);
				return new Rectangle(left, top, width, height);
			}
		}

		public bool CollidesWith(SpriteGameObject obj)
		{
			// Early-out if one sprite is not visible or the bounding boxes don't intersect.
			if (!visible || !obj.visible || !BoundingBox.Intersects(obj.BoundingBox))
			{
				return false;
			}

			// If per-pixel detection is disabled, a bounding box collision is sufficient.
			if (!PerPixelCollisionDetection)
			{
				return true;
			}

			// Avoid self-collision.
			if (this == obj)
			{
				return false;
			}

			// Get the intersection rectangle between both bounding boxes.
			Rectangle intersection = Collision.Intersection(BoundingBox, obj.BoundingBox);

			for (int x = 0; x < intersection.Width; x++)
			{
				for (int y = 0; y < intersection.Height; y++)
				{
					// Calculate the pixel position within this sprite's texture.
					int thisPixelX = (int)Math.Floor((intersection.X + x - (GlobalPosition.X - origin.X * Scale)) / Scale);
					int thisPixelY = (int)Math.Floor((intersection.Y + y - (GlobalPosition.Y - origin.Y * Scale)) / Scale);

					// Calculate the pixel position within the other sprite's texture.
					int objPixelX = (int)Math.Floor((intersection.X + x - (obj.GlobalPosition.X - obj.origin.X * obj.Scale)) / obj.Scale);
					int objPixelY = (int)Math.Floor((intersection.Y + y - (obj.GlobalPosition.Y - obj.origin.Y * obj.Scale)) / obj.Scale);

					// Check if both pixels are non-transparent (assuming IsTranslucent returns true for opaque pixels)
					if (sprite.IsTranslucent(thisPixelX, thisPixelY) &&
						obj.sprite.IsTranslucent(objPixelX, objPixelY))
					{
						return true;
					}
				}
			}
			return false;
		}
	}	
}