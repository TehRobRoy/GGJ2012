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

        #endregion
        #region non standard functions

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
            player.createSprite(Content, Vector2.Zero, "Images/Player", 1.0f); //create the player sprite
            enemy.createSprite(Content, Vector2.One, "Images/Player", 0.5f);
            background = Content.Load<Texture2D>("Images/background"); //load background image
            camera.init(player.m_Pos,0.0f, 0.0f); //initalize camera
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
            
            player.update(new Vector2(1.0f));
            enemy.update(new Vector2(1.0f));
            
            size+=0.5f;
            
            if (size > 0)
                playerColour = Color.Red;
            if (size > 40)
                playerColour = Color.Orange;
            if (size > 80)
                playerColour = Color.Yellow;
            
            camera.update(player.m_Pos);
            
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Wheat);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,null,null,null,null,camera.transform(GraphicsDevice));
            spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);
            player.draw(spriteBatch, playerColour, size);
            enemy.draw(spriteBatch, playerColour, 120.0f);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}