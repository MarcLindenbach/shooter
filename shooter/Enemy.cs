using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Enemy
    {
        public Animation EnemyAnimation;
        public Vector2 Position;
        public Vector2 StartPosition;
        public bool Active;
        public int Speed;
        public int Width => EnemyAnimation.FrameWidth;
        public int Height => EnemyAnimation.FrameHeight;
        public int Left => (int)Position.X;
        public int Right => (int)Position.X + Width;
        public int Top => (int)Position.Y;
        public int Bottom => (int)Position.Y + Height;

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
            Reset();
        }

        public void Reset()
        {
            Position = new Vector2(
                game.GraphicsDevice.Viewport.Width + (random.Next(0, 5) * EnemyAnimation.FrameWidth),
                random.Next(0, game.GraphicsDevice.Viewport.Height - EnemyAnimation.FrameHeight));
            StartPosition = Position;
            Speed = random.Next(-5, -1); 
        }

        public void Update(GameTime gameTime)
        {
            Position.X += Speed;
            Position.Y = (float)(Math.Sin((Double)Position.X / 100) * 100) + StartPosition.Y;

            if (Position.X < -EnemyAnimation.FrameWidth)
            {
                Reset();
            }
            EnemyAnimation.Position = Position;
            EnemyAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            EnemyAnimation.Draw(spriteBatch);
        }
    }
}
