﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.System
{
    public class XnaGame : Microsoft.Xna.Framework.Game
    {
        public IGameInitializer Initializer { get; set; }

        public IUpdater RootUpdater { get; set; }

        public IDrawer RootDrawer { get; set; }

        GraphicsDeviceManager graphics;

        TimeKeeper TimeKeeper { get; set; }

        public XnaGame()
        {
            graphics = new GraphicsDeviceManager(this);

            TimeKeeper = new TimeKeeper();
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

            graphics.PreferredBackBufferWidth = Initializer.ScreenWidth;
            graphics.PreferredBackBufferHeight = Initializer.ScreenHeight;
            graphics.PreferMultiSampling = Initializer.MSAAEnable;

            graphics.ApplyChanges();

            IsFixedTimeStep = Initializer.FixedFrameRate;
            IsMouseVisible = Initializer.MouseVisible;

            base.Initialize();
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
            TimeKeeper.XnaGameTime = gameTime;
            RootUpdater.Update(TimeKeeper);
            base.Update(gameTime);
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            TimeKeeper.XnaGameTime = gameTime;
            RootDrawer.Draw(TimeKeeper);
            base.Draw(gameTime);
        }
    }
}
