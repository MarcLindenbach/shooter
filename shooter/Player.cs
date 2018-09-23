using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Player
    {
        public Animation PlayerAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public int Width => PlayerAnimation.FrameWidth;
        public int Height => PlayerAnimation.FrameHeight;

        public int Left => (int)Position.X;
        public int Right => (int)Position.X + Width;
        public int Top => (int)Position.Y;
        public int Bottom => (int)Position.Y + Height;

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            Position = position;
            Active = true;
            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}