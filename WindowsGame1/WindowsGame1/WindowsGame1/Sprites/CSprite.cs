using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Nebula
{
    class CSprite
    {
        public Vector2 m_Pos; //position
        public Texture2D m_Text; //texture of sprite
        public float moveSpeed; //movement speed of sprite
        public int framecount; //number of animation steps
        private float TimePerFrame; //time between each animation frame
        private int Frame; //frame number
        private float TotalElapsed; //total time elapsed
        private bool Paused; //is animation paused?
        public int FrameWidth;
        public Rectangle sourcerect;
        public bool alive = false;
        
        public void createSprite(ContentManager content, Vector2 position, String texName, float move, int frameCount, int framesPerSec)
        {
            m_Pos = position; //set position to user input
            m_Text = content.Load<Texture2D>(texName); //set texture to texture name
            moveSpeed = move; //set movement speed
            framecount = frameCount; //set the number of frames in the sprite
            TimePerFrame = (float)1 / framesPerSec; //calculate number of frames each second
            Frame = 0; //set frame to 1st frame
            TotalElapsed = 0; //set time elapsed to 0
            Paused = false; //set animation active
            
        }
        
        public virtual void update(Vector2 moveDir, float elapsed)
        {
            //move position with regards to direction + speed
            m_Pos += (moveDir * moveSpeed);
            
            if (Paused)
                return; //if paused don't animate
            TotalElapsed += elapsed; //increase time elapsed
            if (TotalElapsed > TimePerFrame) //check to see 
            {
                Frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                Frame = Frame % framecount;
                TotalElapsed -= TimePerFrame;
            }
        }

        public void DrawFrame(SpriteBatch batch, Vector2 screenPos, float Scale, Color c)
        {
            FrameWidth = m_Text.Width / framecount;
            sourcerect = new Rectangle(FrameWidth * Frame, 0,
                FrameWidth, m_Text.Height);
            //draw sprite at animation step
            batch.Draw(m_Text, screenPos, sourcerect, c,
                0, new Vector2(FrameWidth/2, m_Text.Height/2), Scale, SpriteEffects.None, 0);
        }

        #region animation controllers
        public bool IsPaused
        {
            get { return Paused; }
        }
        public void Reset()
        {
            Frame = 0;
            TotalElapsed = 0f;
        }
        public void Stop()
        {
            Pause();
            Reset();
        }
        public void Play()
        {
            Paused = false;
        }
        public void Pause()
        {
            Paused = true;
        }
        #endregion
    }
}
