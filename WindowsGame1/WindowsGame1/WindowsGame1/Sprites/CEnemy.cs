using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Nebula
{
    class CEnemy:CSprite
    {
        public float scale;
        //public Color[] enemyTextureData;
        public Vector2 move;
        public CEnemy()
        {
            this.m_Pos = Vector2.Zero;
            this.moveSpeed = 0;
        }

        public override void update(Vector2 moveDir, float elapsed)
        {

            base.update(moveDir, elapsed);
        }
        public void draw()
        {
            //enemyTextureData;
        }

    }
}
