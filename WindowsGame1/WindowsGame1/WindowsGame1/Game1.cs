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

        
        SpriteFont debugFont;

        CPlayer player;
        List<CEnemy> enemies;
        List<CObject> statObjects;
        float size;
        Color playerColour;
        
        CCamera camera;

        Texture2D background;
        Texture2D bar;
        int barhealth;
        CAudio audioPlayer;
        Random rand = new Random();
        Vector2 move;
        KeyboardState lastState;
        SamplerState sstate;

        #endregion
        
        #region non standard functions
        Vector2 randomPosition(float range, float start)
        {
            float x = (float)rand.NextDouble() * range - start;
            float y = (float)rand.NextDouble() * range - start;
            Vector2 randomVec = new Vector2(x, y);
            return randomVec;
        }

        void spawnEnemy()
        {
            for (int i = 0; i < (rand.Next(10) + 1); i++)
            {
                CEnemy e = new CEnemy();
                e.createSprite(Content,randomPosition(1000,500),"Images/sun",2.0f,13,12);
                enemies.Add(e);
            }
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
            camera = new CCamera();
            audioPlayer = new CAudio();
            sstate = new SamplerState();
            enemies = new List<CEnemy>();
            statObjects = new List<CObject>();
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
            player.createSprite(Content, randomPosition(10000,500), "Images/sun", 1.0f, 13, 12); //create the player sprite
            for (int i = 0; i < 50000; i++)
            {
                CEnemy e = new CEnemy();
                e.createSprite(Content, randomPosition(100000, 50000), "Images/enemy spritesheet", 0.5f, 13, 8); //create an enemy
                e.scale = (float)rand.NextDouble();
                enemies.Add(e);
            }
            for (int i = 0; i < rand.Next(10,100); i++)
            {
                CObject o = new CObject();
                o.createSprite(Content, randomPosition(10000, 5000), "Images/nebula sprite", 0.0f, 13, 8);
                statObjects.Add(o);
            }
            background = Content.Load<Texture2D>("Images/background"); //load background image
            bar = Content.Load<Texture2D>("Images/bar");
            barhealth = 0;
            camera.init(player.m_Pos,0.2f, 0.0f); //initalize camera
            audioPlayer.init(Content, "audio");
            size = 0; //set inital size to 0
            debugFont = Content.Load<SpriteFont>("Font/debugFont");
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

            if (k.IsKeyDown(Keys.Up))
                camera.Zoom((camera.zoom + 0.05f));
            if (k.IsKeyDown(Keys.Down))
                camera.Zoom((camera.zoom- 0.05f));
            if (mouse.LeftButton == ButtonState.Pressed)
            {
                move = player.calculatePropulsion(mouse, GraphicsDevice);
            }

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.update(move, elapsed);
            if (enemies.Count == 0)
            {
                spawnEnemy();
            }
            foreach (CEnemy e in enemies)
            {
                e.update(new Vector2(1.0f, 0.5f), elapsed);
            }
            foreach (CObject o in statObjects)
            {
                o.update(Vector2.Zero, elapsed);
            }
            camera.update(player.m_Pos);
            barhealth += rand.Next(5);
            if (k.IsKeyDown(Keys.W))
                barhealth-=100;
            if (barhealth > 100)
                barhealth = 100;
            if (barhealth < 0)
                barhealth = 0;
            size+=1f;
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
            player.DrawFrame(spriteBatch, player.m_Pos, (size/10), playerColour);
            foreach (CEnemy e in enemies)
            {
                e.DrawFrame(spriteBatch, e.m_Pos,e.scale, Color.OrangeRed);
            }
            foreach (CObject o in statObjects)
            {
                o.DrawFrame(spriteBatch, o.m_Pos, 0.2f, Color.LightSkyBlue);
            }
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(debugFont, "Health : ", new Vector2(10, 10), Color.White);
            spriteBatch.Draw(bar, new Vector2(115, 21),new Rectangle(115,21,barhealth, bar.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        
    }
}