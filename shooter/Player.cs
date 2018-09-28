using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Player : Sprite
    {
        public Animation PlayerAnimation;
        public bool Active;
        public int Health;

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            Position = position;
            Active = true;
            Health = 100;
            Height = animation.FrameHeight;
            Width = animation.FrameWidth;
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