using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Sokoban
{
    class ControllerGame
    {
        int currentTimeUpdate = 0; // сколько времени прошло
        int periodUpdate = 90; // период обновления в миллисекундах
        GameState gameState;
        SettingsManager settings;
        public delegate void CreateMap();
        CreateMap createMap;

        public ControllerGame(GameState gameState, SettingsManager settings, CreateMap del)
        {
            this.gameState = gameState;
            this.settings = settings;
            createMap = del;
        }

        public void Update(GameTime gameTime, Map map, KeyboardState keyboardState, Player player)
        {
            currentTimeUpdate += gameTime.ElapsedGameTime.Milliseconds;
            
            if (currentTimeUpdate > periodUpdate)
            {
                currentTimeUpdate -= periodUpdate;
                UpdateGame(map, keyboardState, player);
            }
        }

        private void UpdateGame(Map map, KeyboardState keyboardState, Player player)
        {
            if (!gameState.WinLevel)
            {
                UpdatePlayer(map, keyboardState, player);
                CheckWinLevel(map);
            }
            else if (keyboardState.IsKeyDown(Keys.Enter))
            {
                gameState.CurrentLevel++;
                if (gameState.CurrentLevel > settings.CountLevel)
                {
                    gameState.CurrentLevel = 1;
                }
                gameState.WinLevel = false;
                settings.UpdateCurrentLevel(gameState.CurrentLevel);
                createMap();
            }
        }

        private void UpdatePlayer(Map map, KeyboardState keyboardState, Player player)
        {
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                MoveSprite(map, player, -1, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Right))
            {
                MoveSprite(map, player, 1, 0);
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                MoveSprite(map, player, 0, -1);
            }

            else if (keyboardState.IsKeyDown(Keys.Down))
            {
                MoveSprite(map, player, 0, 1);
            }
        }

        private void CheckWinLevel(Map map)
        {
            if (map.CountOpenPlaceX == 0)
            {
                gameState.WinLevel = true;
            }
        }

        private void MoveSprite(Map map, Player player, int deltaX, int deltaY)
        {
            var oldPosition = player.PositionMap;
            var newPosition = player.PositionMap;

            newPosition.X += deltaX;
            newPosition.Y += deltaY;

            if (IsCanMoveNext(map, newPosition) && !IsNextWall(map, newPosition))
            {
                if (IsNextBox(map, newPosition))
                {
                    Point newPositionBox = newPosition;
                    newPositionBox.X = newPosition.X + deltaX;
                    newPositionBox.Y = newPosition.Y + deltaY;

                    if (IsCanMoveNext(map, newPositionBox) && !IsNextWall(map, newPositionBox) && !IsNextBox(map, newPositionBox))
                    {
                        var oldPositionBox = newPosition;
                        var spriteBox = map.GetSprite(oldPositionBox);
                        MoveSpriteInMap(map, spriteBox, oldPositionBox, newPositionBox);
                        player.PositionMap = newPosition;
                        MoveSpriteInMap(map, player.Sprite, oldPosition, newPosition);
                    }
                }
                else
                {
                    player.PositionMap = newPosition;
                    MoveSpriteInMap(map, player.Sprite, oldPosition, newPosition);
                }
            }
        }

        private static void MoveSpriteInMap(Map map, Sprite sprite, Point oldPosition, Point newPosition)
        {
            map.SetSprite(sprite, newPosition);
            map.RemoveSprite(oldPosition);
        }

        private bool IsCanMoveBox(Map map, Point newPosition)
        {
            var spriteType = map.GetTypeSprite(newPosition);
            return spriteType == SpriteType.Box;
        }

        private bool IsNextBox(Map map, Point newPosition)
        {
            var spriteType = map.GetTypeSprite(newPosition);
            return spriteType == SpriteType.Box;
        }

        private bool IsCanMoveNext(Map map, Point newPosition)
        {
            return newPosition.X >= 0 && newPosition.Y >= 0
                && newPosition.X <= map.Width - 1
                && newPosition.Y <= map.Height - 1;
        }

        private bool IsNextWall(Map map, Point newPosition)
        {
            var spriteType = map.GetTypeSprite(newPosition);
            return spriteType == SpriteType.Wall;
        }
    }
}
