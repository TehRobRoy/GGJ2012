using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputLib
{
    public interface IInputHandler
    {
        KeyboardHandler KeyboardState { get; }

        GamePadState[] GamePads { get; }

        ButtonHandler ButtonHandler { get; }

#if !XBOX360
        MouseState MouseState { get; }
        MouseState PreviousMouseState { get; }
#endif
    };

    public class InputHandler : Microsoft.Xna.Framework.GameComponent, IInputHandler
    {
        private KeyboardHandler keyboard;
        private ButtonHandler gamePadHandler = new ButtonHandler();

#if !XBOX360
        private MouseState mouseState;
        private MouseState prevMouseState;
#endif

        public InputHandler(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IInputHandler), this);

            // Initialize our member fields
            keyboard = new KeyboardHandler();

#if !XBOX360
            Game.IsMouseVisible = true;
            prevMouseState = Mouse.GetState();
#endif
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update();

            gamePadHandler.Update();

            //if (keyboard.IsKeyDown(Keys.Escape))
            //    Game.Exit();

            //if (gamePadHandler.WasButtonPressed(0, Buttons.Back))
            //    Game.Exit();

#if !XBOX360
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
#endif

            base.Update(gameTime);
        }

        public KeyboardHandler KeyboardState
        {
            get { return (keyboard); }
        }

        public ButtonHandler ButtonHandler
        {
            get { return (gamePadHandler); }
        }

        public GamePadState[] GamePads
        {
            get { return (gamePadHandler.GamePads); }
        }

#if !XBOX360
        public MouseState MouseState
        {
            get { return (mouseState); }
        }

        public MouseState PreviousMouseState
        {
            get { return (prevMouseState); }
        }
#endif

    }
}