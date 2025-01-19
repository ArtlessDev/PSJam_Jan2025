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
        Texture2D _texture;
        
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
            //moveOne.onClick  = GameMaster.
            moveOne.Rect = new(new Point(320, 480), moveOne.Size);
            moveOne.Texture = Content.Load<Texture2D>("blank-button");
            

            // TODO: use this.Content to load your game content here
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
                Debug.WriteLine("phase: " + GameMaster.CurrentPhase);
                //Debug.WriteLine("can change phases: " + GameMaster.AbleToChangePhases);

            }

            //if (moveOne.onClick != null && onClick.GetInvocationList().Length > 0)
            //{
            //    OnClick(new EventArgs());
            //}

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

            //_spriteBatch.Draw(passTurnButton.Texture, new Vector2(0, 0), Color.White);
            _spriteBatch.Draw(passTurnButton.Texture, passTurnButton.Position, Color.White);
            _spriteBatch.Draw(moveOne.Texture, moveOne.Position, Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
