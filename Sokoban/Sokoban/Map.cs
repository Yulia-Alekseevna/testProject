using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban
{
    class Map
    {       
        public int CountOpenPlaceX { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        private Stack<Sprite>[,] sprites;
        int deltaHeight;
        int deltaWidth;
        readonly int sizeSprite;
        SettingsManager settings;
        Dictionary<SpriteType, Texture2D> dictTexture;
        
        Player player;

        public Map(SettingsManager settings, Dictionary<SpriteType, Texture2D> dictTexture, Player player)
        {
            this.player = player;
            this.dictTexture = dictTexture;
            this.settings = settings;
            sizeSprite = settings.SizeSprite;
        }

        public void CreateMap(int currentLevel)
        {
            try
            {
                string path = "Levels/level" + currentLevel + ".txt";
                using (StreamReader sr = new StreamReader(path))
                {
                    var lineArray = sr.ReadLine().Split(' ');
                    Width = int.Parse(lineArray[0]);
                    Height = int.Parse(lineArray[1]);

                    sprites = new Stack<Sprite>[Height, Width];
                    deltaHeight = (settings.Height - Height * sizeSprite) / 2;
                    deltaWidth = (settings.Width - Width * sizeSprite) / 2;
                                        
                    CreateLevel(sr, settings.SizeSprite);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void CreateLevel(StreamReader sr, int sizeSprite)
        {
            int i = 0;
            int j = 0;
            int x = 0;
            int y = 0;

            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine();
                foreach (char c in line)
                {
                    if (c == 'w')
                    {
                        var sprite = new Sprite(dictTexture[SpriteType.Wall], new Vector2(x, y), SpriteType.Wall);
                        SetSprite(sprite, new Point(j, i));
                    }
                    else if (c == 'b')
                    {
                        var sprite = new Sprite(dictTexture[SpriteType.Box], new Vector2(x, y), SpriteType.Box);
                        SetSprite(sprite, new Point(j, i));
                    }
                    else if (c == 'x')
                    {
                        var sprite = new Sprite(dictTexture[SpriteType.PlaceX], new Vector2(x, y), SpriteType.PlaceX);
                        SetSprite(sprite, new Point(j, i));
                    }

                    else if (c == 'p')
                    {
                        var sprite = new Sprite(dictTexture[SpriteType.Player], new Vector2(x, y), SpriteType.Player);
                        player.PositionMap = new Point(j, i);
                        player.Sprite = sprite;
                        SetSprite(player.Sprite, player.PositionMap);
                    }

                    j++;
                    x += sizeSprite;
                }
                i++;
                j = 0;
                x = 0;
                y += sizeSprite;
            }
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
