using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Enemy : Sprite
    {
        public Animation EnemyAnimation;
        public Vector2 StartPosition;
        public bool Active;
        public int Speed;
        public int Amplitude;

        private Random random;
        private Game game;

        public void Initialize(Game game, Random random)
        {
            this.random = random;
            this.game = game;
            Texture2D texture = game.Content.Load<Texture2D>("Graphics\\mineAnimation");
            EnemyAnimation = new Animation();
            EnemyAnimation.Initialize(
                texture,
                Vector2.Zero,
                47,
                61,
                8,
                60, 
                Color.White, 
                1f, 
                true);
            Active = true;
            Width = texture.Width;
            Height = texture.Height;
            Reset();
        }

        public void Reset()
        {
            Position = new Vector2(
                game.GraphicsDevice.Viewport.Width + (random.Next(0, 5) * EnemyAnimation.FrameWidth),
                random.Next(0, game.GraphicsDevice.Viewport.Height - EnemyAnimation.FrameHeight));
            StartPosition = Position;
            Speed = random.Next(-5, -1);
            Amplitude = random.Next(0, 100);
        }

        public void Update(GameTime gameTime)
        {
            EnemyAnimation.Update(gameTime);

            if (!Active) return;

            Position.X += Speed;
            Position.Y = (float)(Math.Sin((Double)Position.X / 100) * Amplitude) + StartPosition.Y;

            if (Position.X < -EnemyAnimation.FrameWidth)
            {
                Reset();
            }
            EnemyAnimation.Position = Position;
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            EnemyAnimation.Draw(spriteBatch);
        }
    }
}
