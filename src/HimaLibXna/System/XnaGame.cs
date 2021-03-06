﻿using System;
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

        IGameInitializer Initializer { get; set; }

        public IUpdater RootUpdater { get; set; }

        public IDrawer RootDrawer { get; set; }

        public bool VSyncEnable
        {
            get { return graphics.SynchronizeWithVerticalRetrace; }
            set { graphics.SynchronizeWithVerticalRetrace = value; }
        }

        public bool MSAAEnable
        {
            get { return graphics.PreferMultiSampling; }
            set { graphics.PreferMultiSampling = value; }
        }

        public bool IsFullScreen
        {
            get { return graphics.IsFullScreen; }
            set { graphics.IsFullScreen = value; }
        }

        public int ResolutionWidth
        {
            get { return graphics.PreferredBackBufferWidth; }
            set { graphics.PreferredBackBufferWidth = value; }
        }

        public int ResolutionHeight
        {
            get { return graphics.PreferredBackBufferHeight; }
            set { graphics.PreferredBackBufferHeight = value; }
        }

        GraphicsDeviceManager graphics;

        bool disposed = false;

        public bool ApplyGraphicsChangesFlag { get; set; }

        public XnaGame(IGameInitializer initializer)
        {
            Content.RootDirectory = "Content";

            Instance = this;

            Initializer = initializer;

            Initializer.Initialize();

            // コンストラクタ後にGraphicsDeviceが生成されるので、
            // GraphicsDeviceManagerはInitializeの前にコンストラクタで作ること
            graphics = new GraphicsDeviceManager(this);

            // 解像度等の設定もInitializeの前にやっておく
            InitializeGraphics();

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
            InitializeProperty();

            InitializeSystemProperty();

            GraphicsOption.Create();

            foreach (var displayMode in GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                DebugPrint.PrintLine(string.Format(
                    "{0} {1}x{2} ({3})",
                    displayMode.Format.ToString(),
                    displayMode.Width,
                    displayMode.Height,
                    displayMode.AspectRatio));

                if (displayMode.Format == SurfaceFormat.Color)
                {
                    GraphicsOptionBase.Instance.Resolutions.Add(new Resolution()
                    {
                        Width = displayMode.Width,
                        Height = displayMode.Height,
                    });
                }
            }

            base.Initialize();
        }

        void InitializeGraphics()
        {
            graphics.SynchronizeWithVerticalRetrace = Initializer.VSyncEnable;
            graphics.PreferredBackBufferWidth = Initializer.ScreenWidth;
            graphics.PreferredBackBufferHeight = Initializer.ScreenHeight;
            graphics.PreferMultiSampling = Initializer.MSAAEnable;
            graphics.IsFullScreen = Initializer.IsFullScreen;
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
            LoadProfiler.StartFrame();
            LoadProfiler.BeginMark("Update");

            // ApplyChanges()はUpdateの中で呼ぶこと
            // レンダリング中に呼ばれるとまずいので最初に呼んでおく
            if (ApplyGraphicsChangesFlag)
            {
                graphics.ApplyChanges();
                ApplyGraphicsChangesFlag = false;
            }

            TimeKeeper.Instance.XnaGameTime = gameTime;
            MMDXCore.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            RootUpdater.Update();
            base.Update(gameTime);

            LoadProfiler.EndMark();
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            LoadProfiler.BeginMark("Draw");

            TimeKeeper.Instance.XnaGameTime = gameTime;

            RootDrawer.Draw();

            ResetFloatSampler();

            base.Draw(gameTime);

            LoadProfiler.EndMark();
        }

        /// <summary>
        /// XNA4.0のバグ回避のため浮動小数点数バッファのサンプラステートをリセット
        /// </summary>
        void ResetFloatSampler()
        {
            foreach (var i in Enumerable.Range(0, 4))
            {
                GraphicsDevice.SamplerStates[i] = Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp;
                GraphicsDevice.VertexSamplerStates[i] = Microsoft.Xna.Framework.Graphics.SamplerState.PointClamp;
            }
        }
    }
}
