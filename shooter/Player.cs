using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Player
    {
        public Animation PlayerAnimation;
        public Sprite PlayerSprite;
        public bool Active;
        public int Health;

        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;
            PlayerSprite = new Sprite(position, animation.FrameWidth, animation.FrameHeight);
            Active = true;
            Health = 100;
        }

        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = PlayerSprite.Position;
            PlayerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            PlayerAnimation.Draw(spriteBatch);
        }
    }
}