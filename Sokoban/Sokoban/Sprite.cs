using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sokoban
{
    class Sprite
    {
        private Texture2D textureImage;
        public SpriteType typeSprite;
        public Vector2 position;

        public Sprite(Texture2D texture, Vector2 position, SpriteType type)
        {
            textureImage = texture;
            this.position = position;
            typeSprite = type;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, Color.White);
        }

    }
}
