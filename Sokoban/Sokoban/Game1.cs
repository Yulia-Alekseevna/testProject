using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sokoban.Menu;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map map;
        Player player;
        ControllerGame controllerGame;
        MenuGame menu;
        GameState gameState;

        Dictionary<SpriteType, Texture2D> dictTexture;
        SpriteFont font;
        SettingsManager settings;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            settings = new SettingsManager();
            graphics.PreferredBackBufferHeight = settings.Height + settings.HeightFooter;
            graphics.PreferredBackBufferWidth = settings.Width;
            graphics.ApplyChanges();

            dictTexture = new Dictionary<SpriteType, Texture2D>();
            
            gameState = new GameState(State.Menu, false, settings.CurrentLevel);
            controllerGame = new ControllerGame(gameState, settings, CreateMap);
            menu = new MenuGame(gameState, ExitGame, CreateMap);
                        
            var sprite = new Sprite(null,Vector2.Zero, SpriteType.Player);
            player = new Player(sprite, new Point(0, 0));

            map = new Map(settings, dictTexture, player);

            base.Initialize();
        }

        public void ExitGame()
        {
            Exit();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load texture
            dictTexture[SpriteType.Player] = Content.Load<Texture2D>("player");
            dictTexture[SpriteType.Box] = Content.Load<Texture2D>("box");
            dictTexture[SpriteType.Wall] = Content.Load<Texture2D>("wall");
            dictTexture[SpriteType.PlaceX] = Content.Load<Texture2D>("plaseX");
            font = Content.Load<SpriteFont>("MenuFont");

            menu.LoadContent(Content);
        }

        private void CreateMap()
        {
            map.CreateMap(gameState.CurrentLevel);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                gameState.StateGame = State.Menu;
            }
            else if (gameState.StateGame == State.Game)
            {
                controllerGame.Update(gameTime, map, keyboardState, player);
            }
            else
            {
                menu.Update();
            }

            base.Update(gameTime);
        }

        private void DrawGame(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();

            map.Draw(spriteBatch);      
            spriteBatch.DrawString(font, "Box: " + map.CountOpenPlaceX, new Vector2(100, settings.Height + 40), Color.WhiteSmoke);
            spriteBatch.DrawString(font, "Level: " + gameState.CurrentLevel, new Vector2(settings.Width - 200, settings.Height + 40), Color.WhiteSmoke);

            if (gameState.WinLevel)
            {
                spriteBatch.DrawString(font, "!!!WIN LEVEL!!! ", new Vector2(settings.Width / 2 - 100, settings.Height + 20), Color.MonoGameOrange);
                spriteBatch.DrawString(font, "PRESS ENTER", new Vector2(settings.Width / 2 - 95, settings.Height + 50), Color.MonoGameOrange);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            if (gameState.StateGame == State.Game)
            {
                DrawGame(spriteBatch, gameTime);
            }
            else
            {
                menu.Draw(spriteBatch);
            }
        }
    }
}
