using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InputLib
{
    public class FrameCount
    {
        public int NumberOfColumns;
        public int NumberOfRows;

        public FrameCount(int numberOfColumns, int numberOfRows)
        {
            NumberOfColumns = numberOfColumns;
            NumberOfRows = numberOfRows;
        }
    }
}
