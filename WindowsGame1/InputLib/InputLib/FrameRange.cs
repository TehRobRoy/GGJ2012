using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InputLib
{
    public class FrameRange
    {
        public int FirstFrameX;
        public int FirstFrameY;
        public int LastFrameX;
        public int LastFrameY;

        public FrameRange(int firstFrameX, int firstFrameY, int lastFrameX,
            int lastFrameY)
        {
            FirstFrameX = firstFrameX;
            FirstFrameY = firstFrameY;
            LastFrameX = lastFrameX;
            LastFrameY = lastFrameY;
        }
    }
}
