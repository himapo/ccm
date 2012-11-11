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
using MikuMikuDance.XNA;

namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class MapViewer : MyGameComponent
    {
        DirectionalLight stageMainLight;

        public MapViewer(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.SCENE;

            stageMainLight = new DirectionalLight();

            ChildComponents.Add(new DebugAxis(game, CameraLabel.MapViewer));

            AddComponents();
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            InitializeLight();
        }

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.MAP_VIEWER)
            {
                Enabled = true;
                Visible = true;
                RegisterLight();
                ResetMap();
            }
            else
            {
                Enabled = false;
                Visible = false;
            }
        }

        public override void OnSceneEnd(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.MAP_VIEWER)
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
            // デバッグメニューに戻る
            if (InputManager.GetInstance().IsPush(InputLabel.Exit))
            {
                GetService<ISceneService>().ChangeScene(SceneLabel.BOOT_SCENE);
            }

            if (InputManager.GetInstance().IsPush(InputLabel.Down))
            {
                ResetMap();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            DebugFontManager.GetInstance().DrawString(new DebugFontInfo("Map Viewer", new Vector2(50.0f, 60.0f)));
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
            MapManager.GetInstance().CameraLabel = CameraLabel.MapViewer;
        }
    }
}
