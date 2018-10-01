using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Player : AnimatedSprite
    {
        public bool Active;
        public int Health;

        public void Initialize(Animation animation, Vector2 position)
        {
            SpriteAnimation = animation;
            Position = position;
            Active = true;
            Health = 100;
            Height = animation.FrameHeight;
            Width = animation.FrameWidth;
        }
    }
}