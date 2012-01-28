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
        CSprite player;
        float size;
        Color playerColour;
        
        CCamera camera;

        Matrix worldMat;
        Matrix viewMat;
        Matrix projMat;
        #endregion
        #region non standard functions

        private void initWorld()
        {
            worldMat = Matrix.Identity;

            viewMat = camera.camViewMat;

            projMat = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height,
                0.0f, 20.0f);
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
            player = new CSprite();
            camera = new CCamera();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            player.createSprite(Content, new Vector3(0, 0, 0), "Images/single player star");
            camera.init(player.m_Pos);
            size = 0;
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
            player.update();
            size+=0.2f;
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

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,null,null,null,null,camera.camViewMat);
            player.draw(spriteBatch, playerColour, size);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}


//comment goes here
