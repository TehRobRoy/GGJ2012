using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{
    class CSprite
    {
        public Vector2 m_Pos; //position
        Vector2 m_Dir; //movement direction
        Texture2D m_Texture; //texture of sprite
        Rectangle m_Rect; //rectangle used for drawing sprite
        /// <summary>
        /// 
        /// </summary>
        public CSprite() //general constructor
        {
            m_Pos = new Vector2(0, 0); //set position null
            m_Dir = new Vector2(0, 0); //set direction null
        }

        public void createSprite(ContentManager content, Vector2 position, String texName)
        {
            m_Pos = position; //set position to user input
            m_Texture = content.Load<Texture2D>(texName); //set texture to texture name
        }
        
        public void update(Vector2 moveDir)
        {
            m_Pos.X++;
            //move position with regards to direction
        }
        
        public void draw(SpriteBatch sb, Color c, float size)
        {
            //m_Rect = new Rectangle((int) m_Pos.X, (int) m_Pos.Y, (int) (m_Texture.Width + size), (int) (m_Texture.Height + size));
            sb.Draw(m_Texture, m_Pos, null, c, 0,new Vector2(m_Texture.Width/2,m_Texture.Height/2),(size/100),0,0);
        }

        
    }
}
