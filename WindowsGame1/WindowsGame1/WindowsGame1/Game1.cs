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

        Texture2D background;
        
        CAudio audioPlayer;
        Random rand = new Random();
        Vector2 move;
        KeyboardState lastState;
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
            camera.init(player.m_Pos,0.0f, 0.0f); //initalize camera
            audioPlayer.init(Content, "audio");
            size = 0; //set inital size to 0
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
            if (k.IsKeyDown(Keys.Space) && !(lastState.IsKeyDown(Keys.Space)))
            {
                move = randomPosition(10, 5);
                move.Normalize();
            }
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
            camera.update(campos);
            base.Update(gameTime);
            lastState = k;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Rectangle bg = new Rectangle(0, 0, 30000, 30000);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,SamplerState.LinearWrap,DepthStencilState.None,RasterizerState.CullNone,null,camera.transform(GraphicsDevice));
            spriteBatch.Draw(background,new Vector2(-15000, -15000), bg/*new Rectangle((int)(GraphicsDevice.Viewport.Width * 0.5), 
                                                       (int)(GraphicsDevice.Viewport.Height * 0.5),
                                                       background.Width, background.Height)*/, Color.White,0,Vector2.Zero,1,SpriteEffects.None,1);
            player.draw(spriteBatch, playerColour, size);
            enemy.draw(spriteBatch, playerColour, 120.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}