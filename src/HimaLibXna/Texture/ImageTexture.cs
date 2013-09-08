using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Content;

namespace HimaLib.Texture
{
    public class ImageTexture : ITextureXna, ITexture
    {
        public string Name { get; private set; }

        public Texture2D Texture
        {
            get
            {
                if (TextureData == null)
                {
                    Load();
                }
                return TextureData;
            }
        }

        TextureLoader TextureLoader = new TextureLoader();

        Texture2D TextureData;

        public ImageTexture(string name)
        {
            Name = name;
        }

        public void Load()
        {
            TextureData = TextureLoader.Load(Name);
        }
    }
}
