using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputLib
{
    public class KeyboardHandler
    {
        private KeyboardState prevKeyboardState;
        private KeyboardState KeyboardState;

        public KeyboardHandler()
        {
            prevKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return (KeyboardState.IsKeyDown(key));
        }

        public bool IsHoldingKey(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyDown(key));
        }

        public bool WasKeyPressed(Keys key)
        {
            return (KeyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyUp(key));
        }

        public bool HasReleasedKey(Keys key)
        {
            return (KeyboardState.IsKeyUp(key) &&
                prevKeyboardState.IsKeyDown(key));
        }

        public void Update()
        {
            prevKeyboardState = KeyboardState;

            KeyboardState = Keyboard.GetState();
        }
    }
}
