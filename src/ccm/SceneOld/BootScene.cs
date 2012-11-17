using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class BootScene : MyGameComponent
    {
        struct MenuInfo
        {
            public string name;
            public SceneLabel scene;
        }

        List<MenuInfo> menuList;
        int cursor;

        public BootScene(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.SCENE;

            menuList = new List<MenuInfo>();
            cursor = 0;

            // TODO: ここで子コンポーネントを作成します。
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            menuList.Clear();
            menuList.Add(new MenuInfo { name = "Test Game", scene = SceneLabel.GAME_SCENE });
            menuList.Add(new MenuInfo { name = "Main Game", scene = SceneLabel.TITLE_SCENE });
            menuList.Add(new MenuInfo { name = "Model Viewer", scene = SceneLabel.MODEL_VIEWER });
            menuList.Add(new MenuInfo { name = "Map Viewer", scene = SceneLabel.MAP_VIEWER });

            base.Initialize();
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.BOOT_SCENE)
            {
                Enabled = true;
                Visible = true;
            }
            else
            {
                Enabled = false;
                Visible = false;
            }
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            var inputService = InputManager.GetInstance();

            // 選択したシーンに遷移
            if (inputService.IsPush(InputLabel.OK))
            {
                var sceneService = GetService<ISceneService>();
                sceneService.ChangeScene(menuList[cursor].scene);
            }

            // カーソルの移動
            if (inputService.IsPush(InputLabel.Up))
            {
                if (--cursor == -1)
                    cursor = menuList.Count - 1;
            }
            if (inputService.IsPush(InputLabel.Down))
            {
                if (++cursor == menuList.Count)
                    cursor = 0;
            }

            var debugFont = DebugFontManager.GetInstance();
            debugFont.DrawString(new DebugFontInfo("Boot Scene", new Vector2(50.0f, 60.0f)));
            for (var i = 0; i < menuList.Count; ++i )
            {
                Color fontColor = Color.White;
                if (i == cursor)
                    fontColor = Color.Red;
                debugFont.DrawString(new DebugFontInfo(menuList[i].name, new Vector2(80.0f, 100.0f + 22.0f * i), fontColor, Color.Transparent));
            }
        }
    }
}
