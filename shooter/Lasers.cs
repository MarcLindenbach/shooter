using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    class Lasers
    {
        Texture2D Texture;
        public List<Vector2> Positions;
        int Speed = 50;
        int viewportWidth;
        int elapsedTime;
        int reloadTime = 250;
        public int Height => Texture.Height;
        public int Width => Texture.Width;

        public void Initialise(Texture2D texture, int viewportWidth)
        {
            Texture = texture;
            Positions = new List<Vector2>();
            this.viewportWidth = viewportWidth;
        }

        public void ShootLaser(Vector2 position)
        {
            if (elapsedTime > reloadTime)
            {
                Positions.Add(position);
                elapsedTime = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            for (int i = 0; i < Positions.Count; i++)
            {
                Positions[i] = new Vector2(Positions[i].X + Speed, Positions[i].Y);
            }
            Positions.RemoveAll(position => position.X >= viewportWidth);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Positions.ForEach(position => spriteBatch.Draw(Texture, position, Color.White));
        }
    }
}
