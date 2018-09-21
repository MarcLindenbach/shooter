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

        public void Initialize(Animation animation, Vector2 position, int speed)
        {
            EnemyAnimation = animation;
            Position = position;
            StartPosition = position;
            Active = true;
            Speed = speed;
        }

        public void Update(GameTime gameTime)
        {
            Position.X += Speed;

            if (Position.X < -EnemyAnimation.FrameWidth)
            {
                Position = StartPosition;
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
