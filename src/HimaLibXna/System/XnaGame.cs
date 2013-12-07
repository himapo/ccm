using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MikuMikuDance.XNA;
using HimaLib.Debug;
using HimaLib.Content;

namespace HimaLib.System
{
    public class XnaGame : Microsoft.Xna.Framework.Game
    {
        static XnaGame instance;

        public static XnaGame Instance { get { return instance; } private set { instance = value; } }

        public IGameInitializer Initializer { get; set; }

        public IUpdater RootUpdater { get; set; }

        public IDrawer RootDrawer { get; set; }

        public bool VSyncEnable { get { return graphics.SynchronizeWithVerticalRetrace; } }

        GraphicsDeviceManager graphics;

        bool disposed = false;

        public XnaGame()
        {
            Instance = this;

            graphics = new GraphicsDeviceManager(this);

            DebugSampleAccessor.CreateInstance(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    MMDXModelLoader.Dispose();
                    MMDXCore.Instance.Dispose();
                }
                disposed = true;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            Initializer.Initialize();

            InitializeGraphics();

            InitializeProperty();

            InitializeSystemProperty();

            base.Initialize();
        }

        void InitializeGraphics()
        {
            GraphicsDevice.PresentationParameters.PresentationInterval =
                (Initializer.FixedFrameRate || !Initializer.VSyncEnable)
                ? PresentInterval.Immediate
                : PresentInterval.One;

            graphics.SynchronizeWithVerticalRetrace = Initializer.VSyncEnable;

            graphics.PreferredBackBufferWidth = Initializer.ScreenWidth;
            graphics.PreferredBackBufferHeight = Initializer.ScreenHeight;
            graphics.PreferMultiSampling = Initializer.MSAAEnable;

            ApplyGraphicsChanges();
        }

        public void ApplyGraphicsChanges()
        {
            graphics.ApplyChanges();
        }

        void InitializeProperty()
        {
            IsFixedTimeStep = Initializer.FixedFrameRate;
            TargetElapsedTime = new TimeSpan((long)((1.0f / Initializer.FPS) * 1000 * 1000 * 10));
            IsMouseVisible = Initializer.MouseVisible;
        }

        void InitializeSystemProperty()
        {
            SystemProperty.ScreenWidth = Initializer.ScreenWidth;
            SystemProperty.ScreenHeight = Initializer.ScreenHeight;
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";
            base.LoadContent();
        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
            base.UnloadContent();
        }

        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            TimeKeeper.Instance.XnaGameTime = gameTime;
            MMDXCore.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            RootUpdater.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Gray);

            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            TimeKeeper.Instance.XnaGameTime = gameTime;
            RootDrawer.Draw();
            base.Draw(gameTime);
        }
    }
}
