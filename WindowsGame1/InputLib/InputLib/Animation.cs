using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace InputLib
{
    public class Animation
    {
        private string textureName;
        private FrameRange frameRange;
        private int framesPerSecond;
        private float timePerFrame;

        public float TotalElapsedTime = 0.0f;
        public int FrameWidth;
        public int FrameHeight;
        public int NumberOfFrames;
        public int FramesPerRow;
        public int Frame;
        public bool Paused = false;

        public Animation(string textureName, FrameRange frameRange,
            int framesPerSecond)
        {
            this.textureName = textureName;
            this.frameRange = frameRange;
            this.framesPerSecond = framesPerSecond;
            this.timePerFrame = 1.0f / (float)framesPerSecond;
            this.Frame = 0;
        }

        public string TextureName
        {
            get { return (textureName); }
        }

        public FrameRange FrameRange
        {
            get { return (frameRange); }
        }

        public int FramesPerSecond
        {
            get { return (framesPerSecond); }
        }

        public float TimePerFrame
        {
            get { return (timePerFrame); }
        }
    }
}
