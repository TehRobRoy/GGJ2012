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

        Texture2D background;
        
        CAudio audioPlayer;
        Random rand = new Random();
        Vector2 move;
        KeyboardState lastState;
        InputHandler input;
        SamplerState sstate;
        AnimationManager aManager;
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
            input = new InputHandler(this);
            input.Initialize();
            sstate = new SamplerState();
            aManager = new AnimationManager(this, "Images/");
            Components.Add(aManager);
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
            player.createSprite(Content, randomPosition(1000, -500), "Images/sun", 1.0f, 13, 12); //create the player sprite
            enemy.createSprite(Content, randomPosition(1000, -500), "Images/sun", 0.5f, 13, 8); //create an enemy
            background = Content.Load<Texture2D>("Images/background"); //load background image
            camera.init(player.m_Pos,0.0f, 0.0f); //initalize camera
            audioPlayer.init(Content, "audio");
            size = 0; //set inital size to 0
            sstate.AddressU = TextureAddressMode.Mirror; //create sstate for mirroring bg image
            sstate.AddressV = TextureAddressMode.Mirror; //and mirror in both x and y axis
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
            MouseState mouse = Mouse.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || k.IsKeyDown(Keys.Escape)) 
                this.Exit();

            if (mouse.LeftButton == ButtonState.Pressed)
            {
                move = player.calculatePropulsion(mouse, GraphicsDevice);
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.update(move, elapsed);
            enemy.update(new Vector2(1.0f), elapsed);
            camera.update(player.m_Pos);

            size+=0.01f;
            if (size > 0)
                playerColour = Color.Red;
            if (size > 1)
                playerColour = Color.Orange;
            if (size > 2)
                playerColour = Color.Yellow;
            if (size > 3)
                size = 3;
            
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
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,sstate,DepthStencilState.None,RasterizerState.CullNone,null,camera.transform(GraphicsDevice));
            spriteBatch.Draw(background, new Vector2(-15000,-15000), bg, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            //aManager.Draw(gameTime, "Sun", spriteBatch, player.m_Pos);
            player.DrawFrame(spriteBatch, player.m_Pos, size, playerColour);
            //enemy.draw(spriteBatch, playerColour, 120.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}