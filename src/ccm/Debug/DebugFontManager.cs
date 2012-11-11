using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class DebugFontManager : MyGameComponent
    {
        static DebugFontManager instance;

        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        Texture2D whiteTexture;

        List<DebugFontInfo> infoList;

        public static void CreateInstance(Game game)
        {
            instance = new DebugFontManager(game);
        }

        public static DebugFontManager GetInstance()
        {
            return instance;
        }

        DebugFontManager(Game game)
            : base(game)
        {
            // 前のフレームのをクリアするために他より先に更新
            UpdateOrder = (int)UpdateOrderLabel.DEBUG_FONT;
            DrawOrder = (int)DrawOrderLabel.DEBUG_FONT;

            infoList = new List<DebugFontInfo>();
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Game.Content.Load<SpriteFont>("SpriteFont/Kootenay");

            whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            Color[] whitePixels = new Color[] { Color.White };
            whiteTexture.SetData<Color>(whitePixels);
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。
            infoList.Clear();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (var info in infoList)
            {
                spriteBatch.Draw(whiteTexture, CalcBGRect(info), info.BGColor);
                spriteBatch.DrawString(spriteFont, info.Output, info.Position, info.FontColor);
            }
            spriteBatch.End();
        }

        public void DrawString(DebugFontInfo info)
        {
#if DEBUG
            infoList.Add(info);
#endif
        }

        public void DrawString(string output, float x, float y)
        {
            DrawString(new DebugFontInfo(output, new Vector2(x, y)));
        }

        Rectangle CalcBGRect(DebugFontInfo info)
        {
            var size = spriteFont.MeasureString(info.Output);
            var result = new Rectangle((int)info.Position.X - 4, (int)info.Position.Y, (int)size.X + 8, (int)size.Y);
            return result;
        }

    }
}
