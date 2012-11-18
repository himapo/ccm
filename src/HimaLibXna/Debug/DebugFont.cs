using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HimaLib.Graphics;

namespace HimaLib.Debug
{
    public class DebugFont : GraphicsDeviceUser
    {
        class DebugFontInfo
        {
            public string Output { get; set; }
            public Vector2 Position { get; set; }
            public Color FontColor { get; set; }
            public Color BGColor { get; set; }

            public DebugFontInfo()
            {
                Output = "";
                Position = new Vector2(0.0f, 0.0f);
                FontColor = Color.White;
                BGColor = Color.Transparent;
            }

            public DebugFontInfo(string output, Vector2 position, Color fontColor, Color bgColor)
            {
                Initialize(output, position, fontColor, bgColor);
            }

            public DebugFontInfo(string output, Vector2 position)
            {
                Initialize(output, position, Color.White, Color.Transparent);
            }

            void Initialize(string output, Vector2 position, Color fontColor, Color bgColor)
            {
                Output = output;
                Position = position;
                FontColor = fontColor;
                BGColor = bgColor;
            }
        }

        static DebugFont instance = new DebugFont();

        SpriteBatch spriteBatch;
        
        SpriteFont spriteFont;

        Texture2D whiteTexture;

        List<DebugFontInfo> infoList;

        public static DebugFont GetInstance()
        {
            return instance;
        }

        DebugFont()
        {
            infoList = new List<DebugFontInfo>();
        }

        public void Initialize(string fontName)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            var contentLoader = new Content.SpriteFontLoader();
            spriteFont = contentLoader.Load(fontName);

            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] whitePixels = new Color[] { Color.White };
            whiteTexture.SetData<Color>(whitePixels);
        }

        public void Add(string output, float x, float y)
        {
#if DEBUG
            infoList.Add(new DebugFontInfo(output, new Vector2(x, y)));
#endif
        }

        public void Draw()
        {
            spriteBatch.Begin();
            foreach (var info in infoList)
            {
                spriteBatch.Draw(whiteTexture, CalcBGRect(info), info.BGColor);
                spriteBatch.DrawString(spriteFont, info.Output, info.Position, info.FontColor);
            }
            spriteBatch.End();
        }

        public void Clear()
        {
            infoList.Clear();
        }

        Rectangle CalcBGRect(DebugFontInfo info)
        {
            var size = spriteFont.MeasureString(info.Output);
            var result = new Rectangle((int)info.Position.X - 4, (int)info.Position.Y, (int)size.X + 8, (int)size.Y);
            return result;
        }
    }
}
