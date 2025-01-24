using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        public Character zilla;
        List<CustomGameUI> actions;
        Rectangle bgRect;
        Texture2D bg;
        CustomGameUI[] zones;
        SpriteFont spriteFont;
        Font mainFontText;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GameMaster.CustomContent = Content;
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
            passTurnButton.Position = new(896f, 416f);
            passTurnButton.Size = new Point(256, 96);
            passTurnButton.Rect = new(new Point(896, 416), passTurnButton.Size);
            passTurnButton.Texture = Content.Load<Texture2D>("button");

            bgRect = new Rectangle(0, 0, 1280, 720);
            bg = Content.Load<Texture2D>("kaiju-vs-army");

            zones = new CustomGameUI[] { 
                new CustomGameUI(new Rectangle(192*1, 32, 192, 320)),
                new CustomGameUI(new Rectangle(192*2, 32, 192, 320)),
                new CustomGameUI(new Rectangle(192*3, 32, 192, 320)),
                new CustomGameUI(new Rectangle(192*4, 32, 192, 320)),
                new CustomGameUI(new Rectangle(192*5, 32, 192, 320)),
            };

            moveOne = new CustomGameUI();
            moveTwo = new CustomGameUI();
            moveTwo.Rect = new(new Point(512, 416), moveTwo.Size);
            moveThree = new CustomGameUI();
            moveThree.Rect = new(new Point(128, 544), moveThree.Size);
            moveFour = new CustomGameUI();
            moveFour.Rect = new(new Point(512, 544), moveFour.Size);

            mainFontText = new Font();
            mainFontText.FontSprite = Content.Load<SpriteFont>("NotChunky");

            zilla = new Character(CharacterType.Player);
            // TODO: use this.Content to load your game content here

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

            GameMaster.ChangePhase(actions, mouseRect, mouseState, zilla, zones);

            if (GameMaster.CurrentPhase == GamePhases.PlayerTurn)
            {
                foreach (var action in actions)
                {
                    action.changeColor(mouseRect);
                }
            }

            if (GameMaster.CurrentPhase == GamePhases.SelectZone)
            {
                foreach (var zone in zones)
                {
                    zone.changeColor(mouseRect);
                }
            }

            mainFontText.FontText = GameMaster.thisFont.FontText;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.Peru);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(bg, bgRect, Color.White);
            
            _spriteBatch.Draw(zilla.Texture, zilla.Rect, zilla.TextureColor);

            foreach (var zone in zones)
            {
                _spriteBatch.Draw(zone.Texture, zone.Rect, zone.TextureColor);
            }

            foreach (var enemy in GameMaster.enemyWave)
            {
                _spriteBatch.Draw(enemy.Texture, enemy.Rect, enemy.TextureColor);
            }

            _spriteBatch.DrawString(mainFontText.FontSprite, mainFontText.FontText, mainFontText.FontPosition, mainFontText.FontColor);
            
            _spriteBatch.Draw(passTurnButton.Texture, passTurnButton.Rect, passTurnButton.TextureColor);
            _spriteBatch.Draw(moveOne.Texture, moveOne.Rect, moveOne.TextureColor);
            _spriteBatch.Draw(moveTwo.Texture, moveTwo.Rect, moveTwo.TextureColor);
            _spriteBatch.Draw(moveThree.Texture, moveThree.Rect, moveThree.TextureColor);
            _spriteBatch.Draw(moveFour.Texture, moveFour.Rect, moveFour.TextureColor);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
