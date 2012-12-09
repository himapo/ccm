using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DebugSample;

namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class DebugSampleManager : MyGameComponent
    {
        static DebugSampleManager instance;

        // DebugSample
        DebugManager debugManager;
        DebugCommandUI debugCommandUI;
        FpsCounter fpsCounter;
        TimeRuler timeRuler;

        int barIndex;
        int colorIndex;
        Color[] colorSet;

        public static void CreateInstance(Game game)
        {
            instance = new DebugSampleManager(game);
        }

        public static DebugSampleManager GetInstance()
        {
            return instance;
        }

        DebugSampleManager(Game game)
            : base(game)
        {
            CreateComponents(game);
        }

        [Conditional("DEBUG")]
        void CreateComponents(Game game)
        {
            // デバッグマネージャーの初期化と追加
            debugManager = new DebugManager(game);
            game.Components.Add(debugManager);

            // デバッグマコマンドUIの初期化と追加
            debugCommandUI = new DebugCommandUI(game);

            // デバッグコマンドUIを最上面に表示させる為にDrawOrderを変更する
            debugCommandUI.DrawOrder = (int)DrawOrderLabel.DEBUG_UI;

            // 呼び出しキーの変更
            debugCommandUI.ShowHideKey = Keys.F12;

            game.Components.Add(debugCommandUI);

            // FPSカウンターの初期化と追加
            fpsCounter = new FpsCounter(game);
            game.Components.Add(fpsCounter);

            // タイムルーラーの初期化と追加
            timeRuler = new TimeRuler(game);
            timeRuler.DrawOrder = (int)DrawOrderLabel.DEBUG_UI;
            game.Components.Add(timeRuler);

            colorSet = new Color[]{
                Color.Yellow, Color.Cyan,
                Color.Green, Color.Magenta, Color.Blue, Color.Orange,
                Color.Lime, Color.SkyBlue, Color.DarkOrange,
                Color.Purple, Color.Red, Color.Pink, Color.Violet,
            };
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            InitializeTimeRuler();
            base.Initialize();
        }

        [Conditional("DEBUG")]
        void InitializeTimeRuler()
        {
            timeRuler.Visible = true;
            timeRuler.ShowLog = true;
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。

            base.Update(gameTime);
        }

        [Conditional("DEBUG")]
        public void StartFrame()
        {
            colorIndex = 0;
            timeRuler.StartFrame();
        }

        [Conditional("DEBUG")]
        public void BeginTimeRuler(string markerName)
        {
            timeRuler.BeginMark(barIndex, markerName, colorSet[colorIndex]);
            barIndex++;
            if (++colorIndex >= colorSet.Length)
            {
                colorIndex = 0;
            }
        }

        [Conditional("DEBUG")]
        public void EndTimeRuler(string markerName)
        {
            timeRuler.EndMark(--barIndex, markerName);
        }
    }
}
