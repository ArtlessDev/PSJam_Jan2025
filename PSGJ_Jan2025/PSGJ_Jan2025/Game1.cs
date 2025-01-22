using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PSGJ_Jan2025
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private CustomGameUI passTurnButton, moveOne, moveTwo, moveThree, moveFour;
        Character zilla;
        NPC enemyUnit;
        List<CustomGameUI> actions;
        public static event EventHandler Click;

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

            //this will be the guard function
            passTurnButton = new CustomGameUI();
            passTurnButton.Position = new(560, 480);
            passTurnButton.Rect = new(new Point(560, 480), passTurnButton.Size);
            passTurnButton.Texture = Content.Load<Texture2D>("button");

            moveOne = new CustomGameUI();
            moveOne.Position = new(160, 480);
            moveOne.Rect = new(new Point(160, 480), moveOne.Size);
            moveOne.Texture = Content.Load<Texture2D>("blank-button");

            moveTwo = new CustomGameUI();
            moveTwo.Position = new(320, 480);
            moveTwo.Rect = new(new Point(320, 480), moveTwo.Size);
            moveTwo.Texture = Content.Load<Texture2D>("blank-button");

            moveThree = new CustomGameUI();
            moveThree.Position = new(160, 560);
            moveThree.Rect = new(new Point(160, 560), moveThree.Size);
            moveThree.Texture = Content.Load<Texture2D>("blank-button");

            moveFour = new CustomGameUI();
            moveFour.Position = new(320, 560);
            moveFour.Rect = new(new Point(320, 560), moveFour.Size);
            moveFour.Texture = Content.Load<Texture2D>("blank-button");

            zilla = new Character(CharacterType.Player);
            zilla.Rect = new(128, 128, 128, 128);
            zilla.Texture = Content.Load<Texture2D>("zilla");
            // TODO: use this.Content to load your game content here

            enemyUnit = new NPC();
            enemyUnit.Rect = new(64, 64, 64, 64);
            enemyUnit.Texture = Content.Load<Texture2D>("zilla");

            actions = new List<CustomGameUI>();

            actions.Add(moveOne);
            actions.Add(moveTwo);
            actions.Add(moveThree);
            actions.Add(moveFour);
            actions.Add(passTurnButton);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseExtended.Update();
            MouseStateExtended mouseState = MouseExtended.GetState();
            Rectangle mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            GameMaster.ChangePhase(actions, mouseRect, mouseState);

            foreach (var action in actions)
            {
                action.changeColor(mouseRect, Content);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(zilla.Texture, zilla.Rect, Color.White);

            _spriteBatch.Draw(passTurnButton.Texture, passTurnButton.Position, passTurnButton.TextureColor);
            _spriteBatch.Draw(moveOne.Texture, moveOne.Position, moveOne.TextureColor);
            _spriteBatch.Draw(moveTwo.Texture, moveTwo.Position, moveTwo.TextureColor);
            _spriteBatch.Draw(moveThree.Texture, moveThree.Position, moveThree.TextureColor);
            _spriteBatch.Draw(moveFour.Texture, moveFour.Position, moveFour.TextureColor);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
