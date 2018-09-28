using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    class LaserManager
    {
        Texture2D Texture;
        public List<Sprite> Lasers;
        int Speed = 50;
        int viewportWidth;
        int elapsedTime;
        int reloadTime = 250;
        public int Height => Texture.Height;
        public int Width => Texture.Width;

        public void Initialise(Texture2D texture, int viewportWidth)
        {
            Texture = texture;
            Lasers = new List<Sprite>();
            this.viewportWidth = viewportWidth;
        }

        public void ShootLaser(Vector2 position)
        {
            if (elapsedTime > reloadTime)
            {
                Lasers.Add(new Sprite(position, Width, Height));
                elapsedTime = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            for (int i = 0; i < Lasers.Count; i++)
            {
                Lasers[i].Position = new Vector2(Lasers[i].Position.X + Speed, Lasers[i].Position.Y);
            }
            Lasers.RemoveAll(laser => laser.Position.X >= viewportWidth);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Lasers.ForEach(laser => spriteBatch.Draw(Texture, laser.Position, Color.White));
        }
    }
}
