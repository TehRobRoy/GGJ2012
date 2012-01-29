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

public enum gameState
{
    gs_Menu,
    gs_Game,
    gs_Info,
    gs_Info2,
    gs_Credits,
    gs_gameover
};
namespace Nebula
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region variables
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        gameState gs;

        CPlayer player;
        List<CEnemy> enemies;
        List<CObject> statObjects;
        float size;
        Color playerColour;
        Vector2 move;

        CCamera camera;

        Texture2D background;
        Texture2D bar;
        Texture2D info1, info2, banner, credits;
        int barhealth;

        CAudio audioPlayer;
        Random rand = new Random();


        KeyboardState lastKeys;
        MouseState lastMouse;


        Button startButton, menuButton, exitButton, backButton, nextButton, creditsButton, quit2Button;

        SpriteFont debugFont;
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
                e.createSprite(Content, randomPosition(1000, 500), "Images/Sprites/enemy spritesheet", 2.0f, 13, 12);
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
            startButton = new Button(this, "Images/Buttons/start1",
                                        "Images/Buttons/start2",
                                        "Images/Buttons/start3",
                                        new Vector2(350, 110));
            menuButton = new Button(this, "Images/Buttons/help1",
                                        "Images/Buttons/help2",
                                        "Images/Buttons/help3",
                                        new Vector2(275, 200));
            exitButton = new Button(this, "Images/Buttons/quit1",
                                        "Images/Buttons/quit2",
                                        "Images/Buttons/quit3",
                                        new Vector2(425, 200));
            backButton = new Button(this, "Images/Buttons/back1",
                                        "Images/Buttons/back2",
                                        "Images/Buttons/back3",
                                        new Vector2(80, 450));
            nextButton = new Button(this, "Images/Buttons/next1",
                                        "Images/Buttons/next2",
                                        "Images/Buttons/next3",
                                        new Vector2(730, 450));
            creditsButton = new Button(this, "Images/Buttons/credits1",
                                        "Images/Buttons/credits2",
                                        "Images/Buttons/credits3",
                                        new Vector2(730, 50));
            quit2Button = new Button(this, "Images/Buttons/quit1",
                                        "Images/Buttons/quit2",
                                        "Images/Buttons/quit3",
                                        new Vector2(80, 450));
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            IsMouseVisible = true;
            gs = gameState.gs_Menu;//start game at menu screen
            spriteBatch = new SpriteBatch(GraphicsDevice);// Create a new SpriteBatch, which can be used to draw textures.
            player.createSprite(Content, randomPosition(1000, 500), "Images/Sprites/sun", 1.0f, 13, 12); //create the player sprite
            for (int i = 0; i < 1000; i++)
            {
                CEnemy e = new CEnemy();
                e.alive = true;
                e.move = new Vector2((float)rand.NextDouble() * 2 - 1, (float)rand.NextDouble() * 2 - 1);
                e.createSprite(Content, randomPosition(30000, 15000), "Images/Sprites/enemy spritesheet", 0.5f, 13, 8); //create an enemy
                e.scale = (float)rand.NextDouble() * 10 + 1; //create a random size for the enemy
                enemies.Add(e); //add the enemy to the enemy list
            }
            for (int i = 0; i < rand.Next(10, 100); i++)
            {
                CObject o = new CObject();
                o.createSprite(Content, randomPosition(10000, 5000), "Images/Sprites/nebula sprite", 0.0f, 13, 8); //create static object
                statObjects.Add(o); //add object to sprite list
            }
            background = Content.Load<Texture2D>("Images/background"); //load background image
            bar = Content.Load<Texture2D>("Images/bar"); //load the image used for the health bar
            info1 = Content.Load<Texture2D>("Images/instructions page one");
            info2 = Content.Load<Texture2D>("Images/instructions page two");
            banner = Content.Load<Texture2D>("Images/Nebula");
            credits = Content.Load<Texture2D>("Images/Credits Mockup");
            barhealth = 10; //set initial health to 0
            camera.init(player.m_Pos, 0.2f, 0.0f); //initalize camera
            audioPlayer.init(Content, "audio"); //load in the background music and play it.
            size = 20.0f; //set inital size to 0
            debugFont = Content.Load<SpriteFont>("Font/debugFont"); //create a spritefont used for drawing text on the screen
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
            KeyboardState k = Keyboard.GetState(); //default keyboard and mouse state for taking inputs
            MouseState mouse = Mouse.GetState();
            startButton.Update(mouse, lastMouse);
            exitButton.Update(mouse, lastMouse);
            menuButton.Update(mouse, lastMouse);
            backButton.Update(mouse, lastMouse);
            nextButton.Update(mouse, lastMouse);
            creditsButton.Update(mouse, lastMouse);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || k.IsKeyDown(Keys.Escape))
                this.Exit();

            switch (gs)
            {
                case gameState.gs_Menu:
                    {
                        if (startButton.clicked == true)
                            gs = gameState.gs_Game;
                        if (exitButton.clicked == true)
                            Exit();
                        if (menuButton.clicked == true)
                            gs = gameState.gs_Info;


                        break;
                    }
                case gameState.gs_Info:
                    {

                        if (nextButton.clicked == true)
                            gs = gameState.gs_Info2;
                        if (backButton.clicked == true)
                            gs = gameState.gs_Menu;
                        if (creditsButton.clicked == true)
                            gs = gameState.gs_Credits;
                        break;
                    }
                case gameState.gs_Info2:
                    {
                        if (nextButton.clicked == true)
                            gs = gameState.gs_Info;
                        if (backButton.clicked == true)
                            gs = gameState.gs_Menu;
                        if (creditsButton.clicked == true)
                            gs = gameState.gs_Credits;
                        break;
                    }
                case gameState.gs_Game:
                    {
                        if (k.IsKeyDown(Keys.Up) || (mouse.ScrollWheelValue > lastMouse.ScrollWheelValue)) //if 
                            camera.Zoom((camera.zoom + 0.1f));
                        if (k.IsKeyDown(Keys.Down) || (mouse.ScrollWheelValue < lastMouse.ScrollWheelValue))
                            camera.Zoom((camera.zoom - 0.1f));
                        if (mouse.LeftButton == ButtonState.Pressed)
                        {
                            move = player.calculatePropulsion(mouse, GraphicsDevice);
                        }

                        float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        player.update(move, elapsed);
                        Rectangle playerrec = new Rectangle((int)player.m_Pos.X, (int)player.m_Pos.Y,
                                                            (player.m_Text.Width / player.framecount),
                                                            player.m_Text.Height);
                        
                        foreach (CEnemy e in enemies)
                        {
                            if (e.alive)
                            {
                                e.update(e.move * 3, elapsed);
                                Rectangle rE = new Rectangle((int)e.m_Pos.X, (int)e.m_Pos.Y,
                                                              (e.m_Text.Width / e.framecount),
                                                              e.m_Text.Height);
                                bool coll = false;
                                if (rE.Intersects(playerrec))
                                {
                                    coll = true;
                                    

                                }
                                if (coll == true && (e.scale < player.scale))
                                {
                                    e.alive = false;
                                    size += 3.0f;
                                    barhealth += 5;
                                }
                                else if (coll == true && (e.scale > player.scale))
                                {
                                    e.alive = false;
                                    size -= 3.0f;
                                    barhealth -= 5;
                                }
                            }

                        }
                        foreach (CObject o in statObjects)
                        {
                            o.update(Vector2.Zero, elapsed);
                        }
                        camera.update(player.m_Pos);



                        if (size > 0)
                            playerColour = Color.Blue;
                        if (size > 40)
                            playerColour = Color.Orange;
                        if (size > 80)
                            playerColour = Color.Yellow;

                        if (size < 1 || barhealth < 0)
                            gs = gameState.gs_gameover;
                        
                        break;
                    }
                case gameState.gs_Credits:
                    {
                        if (backButton.clicked == true)
                            gs = gameState.gs_Menu;
                        break;
                    }
                case gameState.gs_gameover:
                    {
                        quit2Button.Update(mouse, lastMouse);
                        if (quit2Button.clicked == true)
                            Exit();
                        break;
                    }
            }

            base.Update(gameTime);
            lastMouse = mouse;
            lastKeys = k;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (gs)
            {
                case gameState.gs_Menu:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);
                        spriteBatch.Draw(banner, new Vector2(20, 20), Color.White);
                        startButton.Draw(spriteBatch);
                        exitButton.Draw(spriteBatch);
                        menuButton.Draw(spriteBatch);
                        spriteBatch.End();

                        break;
                    }
                case gameState.gs_Info:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(info1, GraphicsDevice.Viewport.Bounds, Color.White);
                        backButton.Draw(spriteBatch);
                        nextButton.Draw(spriteBatch);
                        creditsButton.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case gameState.gs_Info2:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(info2, GraphicsDevice.Viewport.Bounds, Color.White);
                        backButton.Draw(spriteBatch);
                        nextButton.Draw(spriteBatch);
                        creditsButton.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case gameState.gs_Game:
                    {
                        Rectangle bg = new Rectangle(0, 0, 30000, 30000);
                        GraphicsDevice.Clear(Color.Black);
                        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, sstate, DepthStencilState.None, RasterizerState.CullNone, null, camera.transform(GraphicsDevice));
                        spriteBatch.Draw(background, new Vector2(-15000, -15000), bg, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                        player.DrawFrame(spriteBatch, player.m_Pos, (size / 10), playerColour);
                        foreach (CEnemy e in enemies)
                        {
                            if (e.alive)
                            {
                                e.DrawFrame(spriteBatch, e.m_Pos, e.scale, Color.OrangeRed);
                            }
                        }
                        foreach (CObject o in statObjects)
                        {
                            o.DrawFrame(spriteBatch, o.m_Pos, 0.2f, Color.LightSkyBlue);
                        }
                        spriteBatch.End();

                        spriteBatch.Begin();
                        spriteBatch.DrawString(debugFont, "Health : ", new Vector2(10, 10), Color.White);
                        spriteBatch.Draw(bar, new Vector2(115, 21), new Rectangle(115, 21, barhealth, bar.Height), Color.White);
                        spriteBatch.End();
                        base.Draw(gameTime);
                        break;
                    }
                case gameState.gs_Credits:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(credits, GraphicsDevice.Viewport.Bounds, Color.White);
                        backButton.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }
                case gameState.gs_gameover:
                    {
                        spriteBatch.Begin();
                        spriteBatch.Draw(credits, GraphicsDevice.Viewport.Bounds, Color.White);
                        quit2Button.Draw(spriteBatch);
                        spriteBatch.End();
                        break;
                    }

            }

        }


    }
}