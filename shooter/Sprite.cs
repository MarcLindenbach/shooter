using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Sprite
    {
        public Vector2 Position;
        public Texture2D Texture;
        public float Width;
        public float Height;

        public float Left => Position.X;
        public float Right => Position.X + Width;
        public float Top => Position.Y;
        public float Bottom => Position.Y + Height;

        public Sprite() {
            Position = new Vector2(0, 0);
            Width = 0f;
            Height = 0f;
        }

        public bool Intersects(Sprite otherSprite)
        {
          return !(Left > otherSprite.Right ||
                   Right < otherSprite.Left ||
                   Top > otherSprite.Bottom ||
                   Bottom < otherSprite.Top);
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
