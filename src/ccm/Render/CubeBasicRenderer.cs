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


namespace ccm
{
    class CubeBasicRenderParameter : RenderParameter
    {
        public DirectionalLight DirectionalLight0 { get; set; }
        public Vector3 AmbientLightColor { get; set; }
    }

    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class CubeBasicRenderer : Renderer
    {      
        PhongShader phong;

        public CubeBasicRenderer(Game game)
            : base(game)
        {
            DrawOrder = (int)DrawOrderLabel.RENDER_CUBE_BASIC;
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            var shaderService = GetService<IShaderService>();
            phong = shaderService.Shaders[ShaderLabel.Phong] as PhongShader;

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

        public override void Draw(GameTime gameTime)
        {
            DebugSampleManager.GetInstance().BeginTimeRuler("RenderCubeBasic");
            foreach (var p in ParamList)
            {
                var param = p as CubeBasicRenderParameter;
                var camera = CameraManager.GetInstance().Get(param.cameraLabel);

                phong.Model = param.model;
                phong.World = param.world;
                phong.View = camera.View;
                phong.Projection = camera.Proj;
                phong.EyePosition = camera.Eye;

                phong.DirLight0Direction = param.DirectionalLight0.Direction;
                phong.DirLight0DiffuseColor = param.DirectionalLight0.DiffuseColor;
                phong.DirLight0SpecularColor = param.DirectionalLight0.SpecularColor;
                phong.AmbientLightColor = param.AmbientLightColor;

                phong.Render();
            }
            DebugSampleManager.GetInstance().EndTimeRuler("RenderCubeBasic");

            ParamList.Clear();

            base.Draw(gameTime);
        }
    }
}
