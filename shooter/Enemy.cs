using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Enemy
    {
        public Animation EnemyAnimation;
        public Animation ExplosionAnimation;
        public Vector2 Position;
        public Vector2 StartPosition;
        public bool Active;
        public int Speed;
        public int Amplitude;
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
            Texture2D explosionTexture = game.Content.Load<Texture2D>("Graphics\\explosion");
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
            ExplosionAnimation = new Animation();
            ExplosionAnimation.Initialize(
                explosionTexture,
                Vector2.Zero,
                134,
                134,
                12,
                45, 
                Color.White, 
                1f, 
                false);
            ExplosionAnimation.Active = false;
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
            Amplitude = random.Next(0, 100);
        }

        public void Explode()
        {
            ExplosionAnimation.Position = Position - new Vector2(47);
            ExplosionAnimation.Active = true;
            EnemyAnimation.Active = true;
            Reset();
        }

        public void Update(GameTime gameTime)
        {
            EnemyAnimation.Update(gameTime);
            ExplosionAnimation.Update(gameTime);

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
            ExplosionAnimation.Draw(spriteBatch);
        }
    }
}
