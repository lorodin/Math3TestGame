﻿using Math3TestGame.Controllers;
using Math3TestGame.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace Math3TestGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        /*Vector2 position = Vector2.Zero;
        float speed = 5f;

        int currentTime = 0; // сколько времени прошло
        int period = 50; // частота обновления в миллисекундах

        int frameWidth = 108;
        int frameHeight = 140;
        Point currentFrame = new Point(0, 0);
        Point spriteSize = new Point(8, 2);*/

        private Controller currentController;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }



        protected override void Initialize()
        {
            base.Initialize();
            changeController(ControllerNames.Start);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            TextureHelper.GetInstance().DefaultSpriteMap = Content.Load<Texture2D>("sprites_map");
            GameConfigs.GetInstance().DefaultFont12 = Content.Load<SpriteFont>("test_font");
            GameConfigs.GetInstance().DefaultFont18 = Content.Load<SpriteFont>("sprite_font18");
            GameConfigs.GetInstance().DefaultFont24 = Content.Load<SpriteFont>("sprite_font24");

            GameConfigs.GetInstance().CurrentGame = this;

            GameConfigs.GetInstance().Center = new Point(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
            GameConfigs.GetInstance().Width = Window.ClientBounds.Width;
            GameConfigs.GetInstance().Height = Window.ClientBounds.Height;

            AudioHelper.GetInstance().SetSong(SongName.BANG, Content.Load<SoundEffect>("bang"));
            AudioHelper.GetInstance().SetSong(SongName.LAZER, Content.Load<SoundEffect>("lazer"));
            AudioHelper.GetInstance().SetSong(SongName.KILL, Content.Load<SoundEffect>("kill2"));
            AudioHelper.GetInstance().BackgroundMusic = Content.Load<Song>("bg_music");

            if (GameConfigs.GetInstance().SoundOn)
            {
                AudioHelper.GetInstance().PlayBackground();
            }
        }

        private void changeController(ControllerNames name)
        {
            switch (name)
            {
                case ControllerNames.Play:
                    currentController = new PlayController();
                    break;
                case ControllerNames.Start:
                    currentController = new StartController();
                    break;
            }

            currentController.OnChangeController += (n) =>
            {
                changeController(n);
            };
        }

        protected override void UnloadContent()
        {

        }

        private bool wasPressed = false;

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            currentController.Update(gameTime.ElapsedGameTime.Milliseconds);

            MouseState currentMouseState = Mouse.GetState();

            if(currentMouseState.LeftButton == ButtonState.Released)
            {
                if (wasPressed) { 
                    wasPressed = false;
                    currentController.MouseClick(currentMouseState.X, currentMouseState.Y);
                }
                else
                {
                    currentController.MouseMove(currentMouseState.X, currentMouseState.Y);
                }
            }

            if (currentMouseState.LeftButton == ButtonState.Pressed && !wasPressed)
            {
                wasPressed = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            currentController.GetRenderer().Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
