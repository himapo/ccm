using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Model;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.System;
using HimaLib.Content;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class SpriteFontRenderer : IFontRendererXna
    {
        GraphicsDevice GraphicsDevice { get { return XnaGame.Instance.GraphicsDevice; } }

        SpriteBatch SpriteBatch;

        Texture2D WhiteTexture;

        Dictionary<string, SpriteFont> SpriteFontDic = new Dictionary<string, SpriteFont>();

        SpriteFontLoader SpriteFontLoader = new SpriteFontLoader();

        FontRenderParameter RenderParam;

        public SpriteFontRenderer()
        {
            Initialize();
        }

        void Initialize()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            WhiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            var whitePixels = new Microsoft.Xna.Framework.Color[] { Microsoft.Xna.Framework.Color.White };
            WhiteTexture.SetData<Microsoft.Xna.Framework.Color>(whitePixels);
        }

        public void SetParameter(FontRenderParameter param)
        {
            RenderParam = param;
        }

        public void Begin()
        {
            SpriteBatch.Begin();
        }

        public void End()
        {
            SpriteBatch.End();
        }

        public void Render(FontXna font)
        {
            SpriteBatch.Draw(
                WhiteTexture,
                CalcBGRect(GetSpriteFont(RenderParam.FontName), font.String, RenderParam.Position),
                MathUtilXna.ToXnaColor(RenderParam.BGColor));

            SpriteBatch.DrawString(
                GetSpriteFont(RenderParam.FontName),
                font.String,
                MathUtilXna.ToXnaVector(RenderParam.Position),
                MathUtilXna.ToXnaColor(RenderParam.FontColor));
        }

        SpriteFont GetSpriteFont(string fontName)
        {
            SpriteFont result;
            if(!SpriteFontDic.TryGetValue(fontName, out result))
            {
                result = SpriteFontLoader.Load(fontName);
                SpriteFontDic[fontName] = result;
            }

            return result;
        }

        Microsoft.Xna.Framework.Rectangle CalcBGRect(SpriteFont spriteFont, string value, Vector2 position)
        {
            var size = spriteFont.MeasureString(value);
            var result = new Microsoft.Xna.Framework.Rectangle(
                (int)position.X - 4,
                (int)position.Y,
                (int)size.X + 8,
                (int)size.Y);
            return result;
        }
    }
}
