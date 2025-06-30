using Blok3Game.Engine.GameObjects;
using Blok3Game.Engine.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Blok3Game.GameObjects
{
    public class AchievementNotification : GameObject
    {
        private TextGameObject titleText;
        private TextGameObject descriptionText;
        private TextGameObject pointsText;
        private double displayTime = 5.0; // Show for 5 seconds
        private double elapsedTime = 0.0;
        private Vector2 targetPosition;
        private Vector2 startPosition;
        private bool isAnimating = true;
        private Rectangle background;

        public AchievementNotification(string title, string description, int points) : base(999) // High layer
        {
            // Position in bottom-right corner
            int width = 300;
            int height = 80;
            targetPosition = new Vector2(
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - width - 20,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - height - 20
            );
            startPosition = new Vector2(targetPosition.X + width, targetPosition.Y);
            position = startPosition;

            background = new Rectangle((int)position.X, (int)position.Y, width, height);

            // Create text elements
            titleText = new TextGameObject("Fonts/SpriteFont", 100)
            {
                Text = $" {title}",
                Position = position + new Vector2(10, 5),
                Color = Color.Gold
            };

            descriptionText = new TextGameObject("Fonts/SpriteFont", 100)
            {
                Text = description,
                Position = position + new Vector2(10, 25),
                Color = Color.White
            };

            pointsText = new TextGameObject("Fonts/SpriteFont", 100)
            {
                Text = $"+{points} points",
                Position = position + new Vector2(10, 50),
                Color = Color.LightGreen
            };
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;

            // Slide in animation
            if (isAnimating && elapsedTime < 0.5)
            {
                float progress = (float)(elapsedTime / 0.5);
                progress = MathHelper.SmoothStep(0, 1, progress);
                position = Vector2.Lerp(startPosition, targetPosition, progress);
                
                // Update text positions
                UpdateTextPositions();
            }
            else if (isAnimating)
            {
                isAnimating = false;
                position = targetPosition;
                UpdateTextPositions();
            }

            // Remove after display time
            if (elapsedTime >= displayTime)
            {
                (Parent as GameObjectList)?.Remove(this);
            }

            // Update background rectangle
            background.X = (int)position.X;
            background.Y = (int)position.Y;

            titleText.Update(gameTime);
            descriptionText.Update(gameTime);
            pointsText.Update(gameTime);
        }

        private void UpdateTextPositions()
        {
            titleText.Position = position + new Vector2(10, 5);
            descriptionText.Position = position + new Vector2(10, 25);
            pointsText.Position = position + new Vector2(10, 50);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw background with border
            DrawingHelper.FillRectangle(background, spriteBatch, new Color(0, 0, 0, 180));
            DrawingHelper.DrawRectangle(background, spriteBatch, Color.Gold);

            titleText.Draw(gameTime, spriteBatch);
            descriptionText.Draw(gameTime, spriteBatch);
            pointsText.Draw(gameTime, spriteBatch);
        }
    }
}
