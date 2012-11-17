using Microsoft.Xna.Framework;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class TitleScene : MyGameComponent
    {
        public TitleScene(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.SCENE;

            // TODO: ここで子コンポーネントを作成します。
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
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.TITLE_SCENE)
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
            if (InputManager.GetInstance().IsPush(InputLabel.OK))
            {
                var sceneService = GetService<ISceneService>();
                sceneService.ChangeScene(SceneLabel.GAME_SCENE);
            }

            DebugFontManager.GetInstance().DrawString(new DebugFontInfo("Cube Captor Miku San", new Vector2(100.0f, 100.0f)));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
