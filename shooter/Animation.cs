﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class Animation
    {
        Texture2D spriteStrip;
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        public bool Active;
        public bool Looping;
        public Vector2 Position;

        public void Initialize(
            Texture2D texture, 
            Vector2 position, 
            int frameWidth,
            int frameHeight,
            int frameCount,
            int frameTime,
            Color color,
            float scale,
            bool looping)
        {
            spriteStrip = texture;
            Position = position;
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frameTime;
            this.color = color;
            this.scale = scale;
            Looping = looping;

            elapsedTime = 0;
            currentFrame = 0;
            Active = true;
        }

        public void Update(GameTime gameTime)
        {
            if (Active == false) return;
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentFrame++;
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    Active &= Looping;
                }
                elapsedTime = 0;
            }

            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            destRect = new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                (int)(FrameWidth * scale),
                (int)(FrameHeight * scale));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
                spriteBatch.Draw(spriteStrip, destRect, sourceRect, color);
        }
    }
}
