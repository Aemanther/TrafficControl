using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrafficControl
{
    class Light
    {
        public bool RedGreen;
        public bool HorizontalVertical;
        private float Orientation = 0f;
        public Vector2 Position;
        private Vector2 Origin;

        public Texture2D LightRedSprite;
        public Texture2D LightGreenSprite;

        public Rectangle Area;

        public void Initialize(Texture2D redlight, Texture2D greenlight, Vector2 position, bool vertical)
        {
            RedGreen = false;
            Position = position;
            HorizontalVertical = vertical;
            LightGreenSprite = greenlight;
            LightRedSprite = redlight;
            Origin.X = LightRedSprite.Width / 2;
            Origin.Y = LightRedSprite.Height / 2;
            if (!vertical)
            {
                Orientation = 1.5f;

                Area = new Rectangle((int)Position.X, (int)Position.Y, 50, 30);
                Position.X += 25f;
                Position.Y += 15f;
            }
            else
            {
                Area = new Rectangle((int)Position.X, (int)Position.Y, 30, 50);
                Position.X += 15f;
                Position.Y += 25f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (RedGreen)
            {
                spriteBatch.Draw(LightGreenSprite, Position, null, Color.White, Orientation, Origin, 1.0f, SpriteEffects.None, 0f);   //LightGreenSprite, Position, null, Color.White, Orientation, Origin, SpriteEffects.None, 0f);
            }
            if (!RedGreen)
            {
                spriteBatch.Draw(LightRedSprite, Position, null, Color.White, Orientation, Origin, 1.0f, SpriteEffects.None, 0f);   //LightGreenSprite, Position, null, Color.White, Orientation, Origin, SpriteEffects.None, 0f);
            }
        }
    }
}
