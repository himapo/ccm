using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HimaLib.Render
{
    public abstract class SkinnedModelRendererXna : IModelRendererXna
    {
        public Texture2D BoneRotationTexture { get; set; }

        public Texture2D BoneTranslationTexture { get; set; }

        public Vector2 BoneTextureSize { get; set; }

        public abstract void SetParameter(ModelRenderParameter param);

        public abstract void RenderStatic(Microsoft.Xna.Framework.Graphics.Model model);

        public abstract void RenderDynamic(Microsoft.Xna.Framework.Graphics.Model model);

        public SkinnedModelRendererXna()
        {
        }
    }
}
