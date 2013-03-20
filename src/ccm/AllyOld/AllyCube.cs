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
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class AllyCube : MyGameComponent
    {
        enum State
        {
            Init,
            Alive,
            Dead,
            Empty,
        }

        enum Pose
        {
            Stand,
            Walk,
            Run,
            Crouch,
        }

        public Vector3 Position { get { return position; } set { position = value; } }
        private Vector3 position;

        public Vector3 Rotation { get { return rotation; } set { rotation = value; } }
        private Vector3 rotation;

        State state;
        Action<GameTime> updateFunc;
        Action<GameTime> drawFunc;
        Model model;
        float speed;
        float distance; // プレイヤーとの距離
        CubeRenderParameter renderParam;
        List<CubeShaderParameter> shaderParamList;

        public AllyCube(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.ALLY;
            DrawOrder = (int)DrawOrderLabel.ALLY;

            renderParam = new CubeRenderParameter();
            shaderParamList = new List<CubeShaderParameter>();

            SetState(State.Init);
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            position.X = GameProperty.gameRand.NextFloat() * 200.0f - 100.0f;
            position.Y = 1.5f;
            position.Z = GameProperty.gameRand.NextFloat() * 200.0f - 100.0f;
            rotation = Vector3.Zero;
            speed = GameProperty.gameRand.NextFloat() * 0.2f + 0.4f;
            distance = GameProperty.gameRand.NextFloat() * 40.0f + 10.0f;

            // マテリアルのシェーダと色を適当に設定
            for (var i = 0; i < 6; ++i)
            {
                var shaderParam = new CubeShaderParameter();

                shaderParam.ShaderType = (CubeShaderType)GameProperty.drawRand.Next(2);

                shaderParam.DiffuseColor = new Vector3(
                    GameProperty.drawRand.NextFloat(),
                    GameProperty.drawRand.NextFloat(),
                    GameProperty.drawRand.NextFloat());
                shaderParam.Alpha = 1.0f;
                shaderParam.EmissiveColor = Vector3.Zero;
                shaderParam.SpecularColor = new Vector3(0.6f, 0.6f, 0.6f);
                shaderParam.SpecularPower = 4.0f;

                shaderParamList.Add(shaderParam);
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            model = ResourceManager.GetInstance().Load<Model>("Model/cube003");

            base.LoadContent();
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            updateFunc(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            drawFunc(gameTime);

            base.Draw(gameTime);
        }

        void SetState(State state)
        {
            if (state == State.Init)
            {
                updateFunc = UpdateInit;
                drawFunc = DrawInit;
            }
            else if (state == State.Alive)
            {
                updateFunc = UpdateAlive;
                drawFunc = DrawAlive;
            }

            this.state = state;
        }

        /// <summary>
        /// シーン開始時の状態に戻すメソッド
        /// </summary>
        void Reset()
        {
            SetState(State.Init);
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.GAME_SCENE)
            {
                Reset();
            }
        }

        void UpdateInit(GameTime gameTime)
        {
            InitializeRenderParameter();

            SetState(State.Alive);
        }

        void DrawInit(GameTime gameTime)
        {
        }

        void UpdateAlive(GameTime gameTime)
        {
            var player = PlayerOld.Player.GetInstance();
            var vecToPlayer = new Vector2(player.Position.X - Position.X, player.Position.Z - Position.Z);
            var distanceSquare = vecToPlayer.LengthSquared();

            if (distanceSquare > distance * distance)
            {
                vecToPlayer.Normalize();

                position.X += vecToPlayer.X * speed;
                position.Z += vecToPlayer.Y * speed;
            }
        }

        void DrawAlive(GameTime gameTime)
        {
            renderParam.world =
                Matrix.CreateScale(1.5f) *
                Matrix.CreateRotationX(rotation.X) *
                Matrix.CreateRotationY(rotation.Y) *
                Matrix.CreateRotationZ(rotation.Z) *
                Matrix.CreateTranslation(position);

            RenderManager.GetInstance().Register(renderParam);
        }

        void InitializeRenderParameter()
        {
            renderParam.cameraLabel = CameraLabel.Game;
            renderParam.model = model;
            renderParam.renderer = RendererLabel.Cube;
            renderParam.world = Matrix.Identity;
            renderParam.DirectionalLight0 = GetService<ILightService>().Get(LightAttribute.StageMain);
            renderParam.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);
            renderParam.ShaderParamList = shaderParamList;
        }
    }
}
