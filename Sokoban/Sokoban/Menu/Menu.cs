using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Sokoban.Menu
{
    class MenuGame
    {
        public List<MenuItem> Items { get; set; }

        GameState gameState;
        SpriteFont font;

        int currentItem;
        KeyboardState oldState;

        public delegate void Exit();
        Exit exit;

        public delegate void CreateMap();
        CreateMap createMap;

        public MenuGame(GameState gameState, Exit exit, CreateMap createMap)
        {
            this.gameState = gameState;
            this.exit = exit;
            this.createMap = createMap;

            MenuItem newGame = new MenuItem("Start Game", NewGame);
            MenuItem resumeGame = new MenuItem("Resume Game", ResumeGame);
            MenuItem exitGame = new MenuItem("Exit", ExitGame);

            resumeGame.Active = false;

            Items = new List<MenuItem>();
            Items.Add(newGame);
            Items.Add(resumeGame);
            Items.Add(exitGame);
        }

        private void ResumeGame()
        {
            gameState.StateGame = State.Game;
        }

        private void ExitGame()
        {
            exit();
        }

        private void NewGame()
        {
            Items[1].Active = true;
            gameState.WinLevel = false;
            gameState.StateGame = State.Game;
            createMap();
        }

        public void LoadContent(ContentManager content)
        {
            font = content.Load<SpriteFont>("MenuFont");
        }

        public void Update()
        {
            KeyboardState state = Keyboard.GetState();
            
            if (state.IsKeyDown(Keys.Enter))
            {
                Items[currentItem].click();
            }
            else
            {
                int delta = 0;
                if (state.IsKeyDown(Keys.Up) && oldState.IsKeyUp(Keys.Up))
                {
                    delta = -1;
                }
                else if (state.IsKeyDown(Keys.Down) && oldState.IsKeyUp(Keys.Down))
                {
                    delta = 1;
                }

                UpdateCurrentItem(state, delta);
            }
        }

        private void UpdateCurrentItem(KeyboardState state, int delta)
        {
            currentItem += delta;
            bool ok = false;

            while (!ok)
            {
                if (currentItem < 0)
                    currentItem = Items.Count - 1;

                else if (currentItem >= Items.Count)
                    currentItem = 0;

                else if (!Items[currentItem].Active)
                    currentItem += delta;
                else ok = true;
            }

            oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            int y = 300;
                        
            foreach (MenuItem item in Items)
            {
                if (item.Active)
                {
                    Color color = Color.White;
                    if (item == Items[currentItem])
                    {
                        color = Color.MonoGameOrange;
                    }

                    spriteBatch.DrawString(font, item.Name, new Vector2(500, y), color);
                    y += 40;
                }
            }

            spriteBatch.End();
        }
    }
}
