using System;
using Microsoft.Xna.Framework;

namespace shooter
{
    public class Sprite
    {
        public Vector2 Position;
        public float Width;
        public float Height;

        public float Left => Position.X;
        public float Right => Position.X + Width;
        public float Top => Position.Y;
        public float Bottom => Position.Y + Height;

        public Sprite(Vector2 position, float width, float height) {
            Position = position;
            Width = width;
            Height = height;
        }
    }
}
