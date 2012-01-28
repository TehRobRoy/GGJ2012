using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using InputLib;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CPlayer player;
        CEnemy enemy;
        float size;
        Color playerColour;
        
        CCamera camera;

        Matrix worldMat;
        Matrix viewMat;
        Matrix projMat;

        SpriteFont font;
        String displayMove;
        Vector2 fontPos;

        Texture2D background;

        ParticleEngine particleEngine;
        
        CAudio audioPlayer;
        Random rand = new Random();
        Vector2 move;
        KeyboardState lastState;

        InputHandler input;
        #endregion
        
        #region non standard functions
        Vector2 randomPosition(float range, float start)
        {
            float x = (float)rand.NextDouble() * range - start;
            float y = (float)rand.NextDouble() * range - start;
            Vector2 randomVec = new Vector2(x, y);
            return randomVec;
        }

        #endregion
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            input = new InputHandler(this);
            Components.Add(input);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new CPlayer();
            enemy = new CEnemy();
            camera = new CCamera();
            audioPlayer = new CAudio();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            IsMouseVisible = true;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.createSprite(Content, randomPosition(1000, -500), "Images/Player", 1.0f); //create the player sprite
            enemy.createSprite(Content, randomPosition(1000, -500), "Images/Player", 0.5f);
            background = Content.Load<Texture2D>("Images/background"); //load background image
            font = Content.Load<SpriteFont>("Fonts/font");
            camera.init(player.m_Pos,0.0f, 0.0f); //initalize camera
            //audioPlayer.init(Content, "audio");
            size = 0; //set inital size to 0
            List<Texture2D> fire = new List<Texture2D>();

            fire.Add(Content.Load<Texture2D>("Images/fireball"));
            particleEngine = new ParticleEngine(fire, new Vector2(60, 68));

            fontPos = new Vector2(move.X , move.Y) ;
            
            displayMove = "hello";

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            KeyboardState k = Keyboard.GetState();
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || k.IsKeyDown(Keys.Escape)) 
                this.Exit();
            //if (k.IsKeyDown(Keys.Space) && !(lastState.IsKeyDown(Keys.Space)))
            //{
            //    move = randomPosition(10, 5);
            //    move.Normalize();
            //}

            //if (k.IsKeyDown(Keys.Left) && !(lastState.IsKeyDown(Keys.Left)))
            //{
            //    move.X -= 5;
            //}

            //if (k.IsKeyDown(Keys.Right) && !(lastState.IsKeyDown(Keys.Right)))
            //{
            //    move.X += 5;
            //}

            //if (k.IsKeyDown(Keys.Up) && !(lastState.IsKeyDown(Keys.Up)))
            //{
            //    move.Y -= 5;
            //}

            //if (k.IsKeyDown(Keys.Down) && !(lastState.IsKeyDown(Keys.Down)))
            //{
            //    move.Y += 5;
            //}
            //GamePadState currentState = GamePad.GetState(PlayerIndex.One);

            //if (currentState.IsConnected && currentState.DPad.Up == ButtonState.Pressed)
            //{
            //    move.Y -= 0.25f;
             
            //}

            //if (currentState.IsConnected && currentState.DPad.Down == ButtonState.Pressed)
            //{
            //    move.Y += 0.25f;

            //}

            //if (currentState.IsConnected && currentState.DPad.Left == ButtonState.Pressed)
            //{
            //    move.X -= 0.25f;
            //}

            //if (currentState.IsConnected && currentState.DPad.Right == ButtonState.Pressed)
            //{
            //    move.X += 0.25f;
            //}

            //if (currentState.IsConnected && currentState.Buttons.B == ButtonState.Pressed)
            //{

            //}

            //if (currentState.IsConnected && currentState.Buttons.A == ButtonState.Pressed)
            //{

            //}

            //if (currentState.IsConnected && currentState.Buttons.X == ButtonState.Pressed)
            //{

            //}


            HandleActionInput();

            player.update(move);
            enemy.update(new Vector2(1.0f));
            Vector2 campos = player.m_Pos;
            size+=0.1f;
            
            if (size > 0)
                playerColour = Color.Red;
            if (size > 8)
                playerColour = Color.Orange;
            if (size > 16)
                playerColour = Color.Yellow;
            if (size > 20)
                size = 20;

            particleEngine.EmitterLocation = new Vector2(player.m_Pos.X, player.m_Pos.Y);
            particleEngine.Update();
           
            camera.update(campos);
            base.Update(gameTime);
            lastState = k;
        }

        
        private bool CheckEnterUp()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool checkUp = input.KeyboardState.WasKeyPressed(Keys.Up);
            checkUp |= input.ButtonHandler.WasButtonPressed(0, Buttons.DPadUp);

            return checkUp;
        }

        private void HandleActionInput()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            bool moveUp = input.KeyboardState.WasKeyPressed(Keys.Up);
            moveUp |= input.ButtonHandler.WasButtonPressed(0, Buttons.DPadUp);

            bool moveDown = input.KeyboardState.WasKeyPressed(Keys.Down);
            moveUp |= input.ButtonHandler.WasButtonPressed(0, Buttons.DPadDown);

            bool moveLeft = input.KeyboardState.IsHoldingKey(Keys.Left);
            //enterKey |= input.ButtonHandler.

            if (moveUp)
            {

                player.m_Pos.X += move.X;
            }
            

            if (moveDown)
            {
                move.Y += 1.0f;
            }

            if (moveLeft)
            {
                move.X -= 1.0f;
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,null,null,null,null,camera.transform(GraphicsDevice));
            spriteBatch.Draw(background, new Rectangle((int)(GraphicsDevice.Viewport.Width * 0.5), 
                                                       (int)(GraphicsDevice.Viewport.Height * 0.5),
                                                       background.Width, background.Height), Color.White);
            particleEngine.Draw(spriteBatch);
            player.draw(spriteBatch, playerColour, size);
            enemy.draw(spriteBatch, playerColour, 120.0f);
            Vector2 fontOrigin = font.MeasureString(displayMove) / 2;
            spriteBatch.DrawString(font, displayMove, fontPos, Color.Black, 0, fontOrigin, 1.0f,SpriteEffects.None, 0.5f); 
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

