using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ccm.CameraOld;


namespace ccm
{
    /// <summary>
    /// 基底 Game クラスから派生した、ゲームのメイン クラスです。
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        double updateFPS;
        double renderFPS;
        double updateTotalTime;
        double renderTotalTime;
        int updateTotalFrame;
        int renderTotalFrame;

        const double UPDATE_FPS_INTERVAL = 0.5; // FPSを更新する間隔[秒]

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = GameProperty.resolutionWidth;
            graphics.PreferredBackBufferHeight = GameProperty.resolutionHeight;
            graphics.PreferMultiSampling = false;

            IsMouseVisible = true;
            IsFixedTimeStep = false;

            // 乱数の初期化
            GameProperty.gameRand.Init(Environment.TickCount);
            GameProperty.drawRand.Init(Environment.TickCount);

            // リソースマネージャの生成
            ResourceManager.CreateInstance(this);

            // 入力マネージャの生成
            InputManager.CreateInstance(this);
            Components.Add(InputManager.GetInstance());

            // 数値アップデータマネージャの生成
            Components.Add(new UpdaterManager(this));

            // シェーダマネージャの生成
            Components.Add(new ShaderManager(this));

            // 描画マネージャの生成
            RenderManager.CreateInstance(this);
            Components.Add(RenderManager.GetInstance());

            // デバッグフォントマネージャの生成
            DebugFontManager.CreateInstance(this);
            Components.Add(DebugFontManager.GetInstance());

            // カメラマネージャの生成
            CameraManager.CreateInstance(this);
            Components.Add(CameraManager.GetInstance());

            // ライトマネージャの生成
            Components.Add(new LightManager(this));

            // スクリプトマネージャの生成
            Components.Add(new ScriptManager(this));

            // 物理マネージャの生成
            //Components.Add(new PhysicsManager(this));

            // コリジョンマネージャの生成
            Components.Add(new CollisionManager(this));

            // マップマネージャの生成
            MapManager.CreateInstance(this);
            Components.Add(MapManager.GetInstance());

            // アイテムマネージャの生成
            Components.Add(new ItemManager(this));

            // 味方マネージャの生成
            Components.Add(new AllyManager(this));

            // 敵マネージャの生成
            Components.Add(new EnemyManager(this));

            // デコ（エフェクト）マネージャの生成
            Components.Add(new DecoManager(this));

            // パーティクルマネージャの生成
            Components.Add(new ParticleManager(this));

            // デバッグUIサンプルの生成
            DebugSampleManager.CreateInstance(this);
            Components.Add(DebugSampleManager.GetInstance());

            // シーンマネージャの生成
            Components.Add(new SceneManager(this));

            // デバッグメニューマネージャの生成
            Components.Add(new DebugMenuManager(this));

            updateFPS = 0.0;
            renderFPS = 0.0;
            updateTotalTime = 0.0;
            renderTotalTime = 0.0;
            updateTotalFrame = 0;
            renderTotalFrame = 0;
        }

        /// <summary>
        /// ゲームが実行を開始する前に必要な初期化を行います。
        /// ここで、必要なサービスを照会して、関連するグラフィック以外のコンテンツを
        /// 読み込むことができます。base.Initialize を呼び出すと、使用するすべての
        /// コンポーネントが列挙されるとともに、初期化されます。
        /// </summary>
        protected override void Initialize()
        {
            // TODO: ここに初期化ロジックを追加します。

            base.Initialize();
        }

        /// <summary>
        /// LoadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// 読み込みます。
        /// </summary>
        protected override void LoadContent()
        {
            //ResourceManager.GetInstance().Load();
        }

        /// <summary>
        /// UnloadContent はゲームごとに 1 回呼び出され、ここですべてのコンテンツを
        /// アンロードします。
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: ここで ContentManager 以外のすべてのコンテンツをアンロードします。
        }

        /// <summary>
        /// ワールドの更新、衝突判定、入力値の取得、オーディオの再生などの
        /// ゲーム ロジックを、実行します。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Update(GameTime gameTime)
        {
            DebugSampleManager.GetInstance().StartFrame();

            // 更新期間、"Update"の測定開始
            DebugSampleManager.GetInstance().BeginTimeRuler("Update");

            // ゲームの終了条件をチェックします。
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: ここにゲームのアップデート ロジックを追加します。
            updateTotalTime += gameTime.ElapsedGameTime.TotalSeconds;
            updateTotalFrame++;
            if (updateTotalTime > UPDATE_FPS_INTERVAL)
            {
                updateFPS = updateTotalFrame / updateTotalTime;
                updateTotalTime = 0.0;
                updateTotalFrame = 0;
                if (updateFPS > 999.99)
                    updateFPS = 999.99;
            }

            base.Update(gameTime);

            // 更新期間、"Update"の測定終了
            DebugSampleManager.GetInstance().EndTimeRuler("Update");
        }

        /// <summary>
        /// ゲームが自身を描画するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        protected override void Draw(GameTime gameTime)
        {
            DebugSampleManager.GetInstance().BeginTimeRuler("Draw");

            GraphicsDevice.Clear(Color.Gray);

            // TODO: ここに描画コードを追加します。
            renderTotalTime += gameTime.ElapsedGameTime.TotalSeconds;
            renderTotalFrame++;
            if (renderTotalTime > UPDATE_FPS_INTERVAL)
            {
                renderFPS = renderTotalFrame / renderTotalTime;
                renderTotalTime = 0.0;
                renderTotalFrame = 0;
                if (renderFPS > 999.99)
                    renderFPS = 999.99;
            }

            ShowFPS();

            base.Draw(gameTime);

            DebugSampleManager.GetInstance().EndTimeRuler("Draw");
        }

        void ShowFPS()
        {
            var debugFont = DebugFontManager.GetInstance();

            var output = String.Format("Update {0:f2}FPS", updateFPS);
            var fontPos = new Vector2(0.0f, 0.0f);
            debugFont.DrawString(new DebugFontInfo(output, fontPos));

            fontPos.Y += 22.0f;
            output = String.Format("Render {0:f2}FPS", renderFPS);
            debugFont.DrawString(new DebugFontInfo(output, fontPos));
        }
    }
}
