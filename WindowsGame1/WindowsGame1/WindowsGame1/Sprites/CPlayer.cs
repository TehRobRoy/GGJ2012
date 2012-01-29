using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Nebula
{
    class CPlayer : CSprite //create player inheriting draw + basic functions from CSprite
    {

        //player variables go here
        Vector2 moveVec;

        public CPlayer()
        {
            this.m_Pos = new Vector2(0, 0); //set position null
            this.moveSpeed = 0;
        }

        public Vector2 calculatePropulsion(MouseState ms, GraphicsDevice gd)
        {
            //take in mouse position
            //calcualte normalised vector
            //get distance with Vector2.distance(mousePos, screen Centre)
            //Scale movement by distance?
            Vector2 mousePos = new Vector2(ms.X,ms.Y);
            Vector2 playerCentre = new Vector2(gd.Viewport.Width/2,gd.Viewport.Height/2);
            Vector2 trans = playerCentre - mousePos;
            trans.Normalize();
            float dist = Vector2.Distance(mousePos, playerCentre);
           // dist = (float)Math.Sqrt(dist);
            moveVec = trans * dist / 40;
            return moveVec;
        }

        //player functions go here
    }
}
