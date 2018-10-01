using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace shooter
{
    public class AnimatedSprite : Sprite
    {
        public Animation SpriteAnimation;

        public virtual void Update(GameTime gameTime) {
            SpriteAnimation.Position = Position;
            SpriteAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            SpriteAnimation.Draw(spriteBatch);
        }
    }
}
