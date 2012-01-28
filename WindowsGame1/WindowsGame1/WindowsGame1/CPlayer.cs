using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{
    class CPlayer : CSprite //create player inheriting draw + basic functions from CSprite
    {

        //player variables go here
        

        public CPlayer()
        {
            this.m_Pos = new Vector2(0, 0); //set position null
            this.moveSpeed = 0;
        }



        public override void update(Vector2 moveDir)
        {
            m_Pos += (moveDir * moveSpeed);

        }

        //player functions go here
    }
}
