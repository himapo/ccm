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
    class ModelViewer : MyGameComponent
    {
        DirectionalLight stageMainLight;
        Model model;
        List<CubeShaderParameter> shaderParamList;

        public ModelViewer(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.SCENE;

            stageMainLight = new DirectionalLight();
            shaderParamList = new List<CubeShaderParameter>();

            // TODO: ここで子コンポーネントを作成します。
            
            AddComponents();
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            for (var i = 0; i < 6; ++i)
            {
                var shaderParam = new CubeShaderParameter();

                shaderParam.DiffuseColor = new Vector3(
                    GameProperty.drawRand.NextFloat(),
                    GameProperty.drawRand.NextFloat(),
                    GameProperty.drawRand.NextFloat());
                shaderParam.Alpha = 1.0f;
                shaderParam.EmissiveColor = Vector3.Zero;
                shaderParam.SpecularColor = new Vector3(0.6f, 0.6f, 0.6f);
                shaderParam.SpecularPower = 16.0f;

                shaderParamList.Add(shaderParam);
            }

            InitializeLight();
        }

        void InitializeLight()
        {
            stageMainLight.AddAttribute(LightAttribute.StageMain);
            stageMainLight.Direction = new Vector3(-1.0f, -1.0f, -1.0f);
            stageMainLight.Direction.Normalize();
            stageMainLight.DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
            stageMainLight.SpecularColor = new Vector3(1.0f, 1.0f, 1.0f);
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.MODEL_VIEWER)
            {
                Enabled = true;
                Visible = true;
                RegisterLight();
                LoadModel();
            }
        }

        public override void OnSceneEnd(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.MODEL_VIEWER)
            {
                Enabled = false;
                Visible = false;
                UnregisterLight();
            }
        }

        void LoadModel()
        {
            model = ResourceManager.GetInstance().Load<Model>("Model/cube003");
        }

        void RegisterLight()
        {
            GetService<ILightService>().Add(stageMainLight);
        }

        void UnregisterLight()
        {
            GetService<ILightService>().Remove(stageMainLight);
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // デバッグメニューに戻る
            if (InputManager.GetInstance().IsPush(InputLabel.Exit))
            {
                GetService<ISceneService>().ChangeScene(SceneLabel.BOOT_SCENE);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            var renderParam = new CubeRenderParameter();
            renderParam.cameraLabel = CameraLabel.ModelViewer;
            renderParam.model = model;
            renderParam.world = Matrix.Identity;
            renderParam.renderer = RendererLabel.Cube;
            renderParam.ShaderParamList = shaderParamList;
            renderParam.DirectionalLight0 = GetService<ILightService>().Get(LightAttribute.StageMain);
            renderParam.AmbientLightColor = new Vector3(0.4f, 0.4f, 0.4f);

            RenderManager.GetInstance().Register(renderParam);

            DebugFontManager.GetInstance().DrawString(new DebugFontInfo("Model Viewer", new Vector2(50.0f, 60.0f)));
        }
    }
}
