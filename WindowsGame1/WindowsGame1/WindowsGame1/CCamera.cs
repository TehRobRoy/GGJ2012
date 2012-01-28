using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame1
{
    class CCamera
    {
        Vector3 position;
        Vector3 lookat;
        public Matrix camViewMat;

        public void init(Vector3 p)
        {
            position = p;
            position.Z += 10;
            lookat = Vector3.Forward;
            camViewMat = Matrix.CreateTranslation(position);
        }
        public void update(Vector3 p)
        {
            position = p;
            position.Z += 10;
            camViewMat = Matrix.CreateTranslation(position);
        }
    }
}