using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace PSGJ_Jan2025
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CustomGameUI passTurnButton, moveOne;
        Character zilla;
        NPC enemyUnit;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            passTurnButton = new CustomGameUI();
            passTurnButton.Position = new(640, 480);
            passTurnButton.Rect = new(new Point(640, 480), passTurnButton.Size);
            passTurnButton.Texture = Content.Load<Texture2D>("button");

            moveOne = new CustomGameUI();
            moveOne.Position = new(320, 480);
            moveOne.Rect = new(new Point(320, 480), moveOne.Size);
            moveOne.Texture = Content.Load<Texture2D>("blank-button");
            
            zilla = new Character(CharacterType.Player);
            zilla.Rect = new(128, 128, 128, 128);
            zilla.Texture = Content.Load<Texture2D>("zilla");
            // TODO: use this.Content to load your game content here

            enemyUnit = new NPC();
            enemyUnit.Rect = new(128, 128, 128, 128);
            enemyUnit.Texture = Content.Load<Texture2D>("zilla");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            MouseState mouseState = Mouse.GetState();
            Rectangle mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);

            if (mouseState.LeftButton == ButtonState.Pressed && mouseRect.Intersects(passTurnButton.Rect) && GameMaster.AbleToChangePhases == true)
            {
                GameMaster.ChangePhase();
            }

            if (mouseState.LeftButton == ButtonState.Pressed && mouseRect.Intersects(moveOne.Rect))
            {
                GameMaster.ResetPhaseChangeFlag();
                Debug.WriteLine("can change phases: " + GameMaster.AbleToChangePhases);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(passTurnButton.Texture, passTurnButton.Position, Color.White);
            _spriteBatch.Draw(moveOne.Texture, moveOne.Position, Color.White);
            _spriteBatch.Draw(zilla.Texture, zilla.Rect, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
