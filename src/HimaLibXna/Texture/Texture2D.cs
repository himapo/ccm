using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Texture
{
    public class Texture2D
    {
        public float Width { get { return texture.Width; } }

        public float Height { get { return texture.Height; } }

        Microsoft.Xna.Framework.Graphics.Texture2D texture;

        public Texture2D(Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            this.texture = texture;
        }
    }
}
