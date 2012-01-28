using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame1
{
    class CEnemy:CSprite
    {
        public CEnemy()
        {
            this.m_Pos = Vector2.Zero;
            this.moveSpeed = 0;
        }

        public override void update(Vector2 moveDir, float elapsed)
        {

            base.update(moveDir, elapsed);
        }

    }
}
