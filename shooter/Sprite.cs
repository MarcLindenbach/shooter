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
        public Rectangle AsRectangle => new Rectangle(
            (int)Left,
            (int)Top,
            (int)Width,
            (int)Height);

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

        public virtual void DrawBoundingBox(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) {
            Texture2D rect = new Texture2D(graphicsDevice, 1, 1);
            rect.SetData(new[] { Color.White });
            spriteBatch.Draw(rect, AsRectangle, Color.Red);
        }
    }
}
