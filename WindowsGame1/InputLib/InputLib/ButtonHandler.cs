using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace InputLib
{
    public class ButtonHandler
    {
        private GamePadState[] prevGamePadsState = new GamePadState[4];
        private GamePadState[] gamePadsState = new GamePadState[4];

        public GamePadState[] GamePads
        {
            get
            {
                return (gamePadsState);
            }
        }

        public ButtonHandler()
        {
            prevGamePadsState[0] = GamePad.GetState(PlayerIndex.One);
            prevGamePadsState[1] = GamePad.GetState(PlayerIndex.Two);
            prevGamePadsState[2] = GamePad.GetState(PlayerIndex.Three);
            prevGamePadsState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public void Update()
        {
            // Set previous state to new state
            prevGamePadsState[0] = gamePadsState[0];
            prevGamePadsState[1] = gamePadsState[1];
            prevGamePadsState[2] = gamePadsState[2];
            prevGamePadsState[3] = gamePadsState[3];

            // Get new state
            gamePadsState[0] = GamePad.GetState(PlayerIndex.One);
            gamePadsState[1] = GamePad.GetState(PlayerIndex.Two);
            gamePadsState[2] = GamePad.GetState(PlayerIndex.Three);
            gamePadsState[3] = GamePad.GetState(PlayerIndex.Four);
        }

        public bool WasButtonPressed(int playerIndex, Buttons button)
        {
            return (gamePadsState[playerIndex].IsButtonDown(button) &&
                prevGamePadsState[playerIndex].IsButtonUp(button));
        }
    }
}
