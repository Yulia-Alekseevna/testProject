using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Sokoban
{
    class Map
    {
        private Stack<Sprite>[,] sprites;
        public int CountOpenPlaceX { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        readonly int deltaHeight;
        readonly int deltaWidth;
        readonly int sizeSprite;
        
        public Map(int widthMap, int heightMap, SettingsManager settings)
        {
            sprites = new Stack<Sprite>[heightMap,widthMap];
            sizeSprite = settings.SizeSprite;
            deltaHeight = (settings.Height - heightMap * sizeSprite) / 2;
            deltaWidth = (settings.Width - widthMap * sizeSprite) / 2;
            
            Height = heightMap;
            Width = widthMap;
        }

        public bool SetSprite(Sprite sprite, Point positinMap)
        {
            if (sprite.typeSprite == SpriteType.PlaceX)
            {
                CountOpenPlaceX++;
            }

            if (sprite.typeSprite == SpriteType.Box)
            {
                CheckPLaceX(sprites[positinMap.Y, positinMap.X]);
            }

            sprite.position.X = positinMap.X * sizeSprite + deltaWidth;
            sprite.position.Y = positinMap.Y * sizeSprite + deltaHeight;

            if (sprites[positinMap.Y, positinMap.X] == null)
            {
                sprites[positinMap.Y, positinMap.X] = new Stack<Sprite>();
            }
            
            sprites[positinMap.Y, positinMap.X].Push(sprite);
            return true;
        }

        private void CheckPLaceX(Stack<Sprite> stack)
        {
            if (stack != null && stack.Count > 0 && stack.Peek().typeSprite == SpriteType.PlaceX)
            {
                CountOpenPlaceX--;
            }
        }

        public bool RemoveSprite(Point positionMap)
        {
            var spriteStack = sprites[positionMap.Y, positionMap.X];
            var spriteRemove = spriteStack.Pop();
            if (spriteRemove.typeSprite == SpriteType.Box && spriteStack.Count > 0 && spriteStack.Peek().typeSprite == SpriteType.PlaceX)
            {
                CountOpenPlaceX++;
            }
            return true;
        }

        public SpriteType GetTypeSprite(Point positinMap)
        {
            var sprite = sprites[positinMap.Y, positinMap.X];

            if (sprite != null && sprite.Count != 0)
                return sprite.Peek().typeSprite;
            else
                return SpriteType.Null;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var sprite in sprites)
            {
                if (sprite != null && sprite.Count != 0)
                    sprite.Peek().Draw(spriteBatch);
            }
        }

        public Sprite GetSprite(Point positinMap)
        {
            var sprite = sprites[positinMap.Y, positinMap.X];

            if (sprite != null && sprite.Count != 0)
            {
                return sprite.Peek();
            }

            return null;
        }
    }
}
