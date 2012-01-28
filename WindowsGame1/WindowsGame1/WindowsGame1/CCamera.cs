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
        public Vector2 position;
        float rotation;
        float zoom;
        Matrix m_Trans;

        public void init(Vector2 p, float z, float rot)
        {
            position = p;
            zoom = z;
            rotation = rot;
        }
        public void update(Vector2 p)
        {
            position = p;
        }

        public Matrix transform(GraphicsDevice graphicsDevice)
        {
            m_Trans = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) *
                                //Matrix.CreateRotationZ(rotation) *
                                //Matrix.CreateScale(new Vector3(zoom, zoom, 1)) *
                                Matrix.CreateTranslation(new Vector3(graphicsDevice.Viewport.Width * 0.5f, graphicsDevice.Viewport.Height * 0.5f, 0));
            return m_Trans;
        }
    }
}