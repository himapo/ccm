using Microsoft.Xna.Framework;
using MikuMikuDance.Core;
using MikuMikuDance.Core.Model;
using MikuMikuDance.Core.Motion;
using MikuMikuDance.XNA;
using ccm.CameraOld;
using ccm.PlayerOld;

namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class GameScene : MyGameComponent
    {
        int frame;

        DirectionalLight stageMainLight;

        public GameScene(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.SCENE;

            frame = 0;

            stageMainLight = new DirectionalLight();

            // TODO: ここで子コンポーネントを作成します。
            PlayerOld.Player.CreateInstance(game);
            ChildComponents.Add(PlayerOld.Player.GetInstance());
            ChildComponents.Add(new DebugAxis(game, CameraLabel.Game));

            AddComponents();
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            InitializeLight();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        void InitializeLight()
        {
            stageMainLight.AddAttribute(LightAttribute.StageMain);
            stageMainLight.Direction = new Vector3(-1.0f, -1.0f, -1.0f);
            stageMainLight.Direction.Normalize();
            stageMainLight.DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
            stageMainLight.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f);
        }

        void RegisterLight()
        {
            GetService<ILightService>().Add(stageMainLight);
        }

        void UnregisterLight()
        {
            GetService<ILightService>().Remove(stageMainLight);
        }

        void ResetMap()
        {
            MapManager.GetInstance().Clear();
            MapManager.GetInstance().Generate();
            MapManager.GetInstance().CameraLabel = CameraLabel.Game;
        }

        void ResetEnemy()
        {
            var enemyService = GetService<IEnemyService>();
            enemyService.Clear();
        }

        void ResetFrame()
        {
            frame = 0;
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.GAME_SCENE)
            {
                Enabled = true;
                Visible = true;
                ResetFrame();
                RegisterLight();
                ResetMap();
                ResetEnemy();
            }
            else
            {
                Enabled = false;
                Visible = false;
            }
        }

        public override void OnSceneEnd(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.GAME_SCENE)
            {
                Enabled = false;
                Visible = false;
                UnregisterLight();
            }
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            var input = InputManager.GetInstance();
            if (input.IsPush(InputLabel.Exit))
            {
                MapManager.GetInstance().Clear();

                var sceneService = GetService<ISceneService>();
                sceneService.ChangeScene(SceneLabel.TITLE_SCENE);

                return;
            }

            // 敵出現
            if (frame % 300 == 0)
            {
                var enemyService = GetService<IEnemyService>();
                enemyService.Add();
            }

            var camera = CameraManager.GetInstance().Get(CameraLabel.Game);

            MMDXCore.Instance.Camera.Position = camera.Eye;
            MMDXCore.Instance.Camera.SetVector(camera.At - camera.Eye);
            MMDXCore.Instance.Camera.FieldOfView = camera.FovY;
            MMDXCore.Instance.Camera.Near = camera.Near;
            MMDXCore.Instance.Camera.Far = camera.Far;

            //MMDのUpdateを呼び出す
            MMDXCore.Instance.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            frame++;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            // MMDModelを所持するPlayerを破棄
            PlayerOld.Player.GetInstance().Dispose();

            //MMDの破棄処理を実行
            MMDXCore.Instance.Dispose();
        }
    }
}
