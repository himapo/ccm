using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class SceneManager : MyGameComponent, ISceneService
    {
        Dictionary<SceneLabel, MyGameComponent> sceneMap;
        SceneLabel currentScene;

        public SceneManager(Game game)
            : base(game)
        {
            sceneMap = new Dictionary<SceneLabel, MyGameComponent>();
            currentScene = SceneLabel.NULL_SCENE;

            // TODO: ここで子コンポーネントを作成します。
            
            AddScene(SceneLabel.TITLE_SCENE, new TitleScene(game));
            AddScene(SceneLabel.GAME_SCENE, new GameScene(game));

            AddDebugScenes(game);

            AddComponents();

            game.Services.AddService(typeof(ISceneService), this);
        }

        [Conditional("DEBUG")]
        void AddDebugScenes(Game game)
        {
            AddScene(SceneLabel.BOOT_SCENE, new BootScene(game));
            AddScene(SceneLabel.MODEL_VIEWER, new ModelViewer(game));
            AddScene(SceneLabel.MAP_VIEWER, new MapViewer(game));
        }

        void AddScene(SceneLabel label, MyGameComponent scene)
        {
            scene.Enabled = false;
            scene.Visible = false;
            sceneMap[label] = scene;
            ChildComponents.Add(scene);
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
#if DEBUG
            var firstScene = SceneLabel.BOOT_SCENE;
#else
            var firstScene = SceneLabel.TITLE_SCENE;
#endif
            ChangeScene(firstScene);

            base.Initialize();
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

        public void ChangeScene(SceneLabel label)
        {
            // すべてのコンポーネントに通知する
            foreach (var component in Game.Components)
            {
                var my = component as MyGameComponent;
                if (my != null)
                {
                    my.OnSceneEnd(currentScene);
                }
            }

            currentScene = label;

            // すべてのコンポーネントに通知する
            foreach (var component in Game.Components)
            {
                var my = component as MyGameComponent;
                if (my != null)
                {
                    my.OnSceneBegin(currentScene);
                }
            }
        }
    }
}
