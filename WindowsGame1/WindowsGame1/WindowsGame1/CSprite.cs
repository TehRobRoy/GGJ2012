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
        public Texture2D m_Text; //texture of sprite
        public float moveSpeed; //movement speed of sprite

        public void createSprite(ContentManager content, Vector2 position, String texName, float move)
        {
            m_Pos = position; //set position to user input
            m_Text = content.Load<Texture2D>(texName); //set texture to texture name
            moveSpeed = move;
        }
        
        public virtual void update(Vector2 moveDir)
        {
            //move position with regards to direction + speed
            m_Pos += (moveDir * moveSpeed);
        }
        
        public void draw(SpriteBatch sb, Color c, float size)
        {
            sb.Draw(m_Text, m_Pos, null, c, 0, new Vector2(m_Text.Width / 2, m_Text.Height / 2), (0.5f + (size / 120)), 0, 0);

        }

        
    }
}
