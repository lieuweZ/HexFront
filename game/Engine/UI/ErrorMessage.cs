
using System;
using Blok3Game.Engine.GameObjects;
using Microsoft.Xna.Framework;

namespace Blok3Game.Engine.UI
{
	public class ErrorMessage : TextGameObject
	{
		private float timeToLive = 5;
		public event Action<ErrorMessage> OnTimerEnd;

		public ErrorMessage(string text, int layer = 0, string id = "") : base("Fonts/SpriteFont@20px", layer, id)
		{
			Text = text;
			Color = Color.Red;
			
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			timeToLive -= (float)gameTime.ElapsedGameTime.TotalSeconds;

			//fade out the alpha value of the text
			Color = new Color(Color, timeToLive / 5);

			//remove the error message after 5 seconds
			if (timeToLive <= 0)
			{
				OnTimerEnd?.Invoke(this);
			}
		}
	}
}