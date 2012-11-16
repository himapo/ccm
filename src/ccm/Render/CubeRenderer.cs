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
    class CubeRenderParameter : RenderParameter
    {
        public DirectionalLight DirectionalLight0 { get; set; }
        public Vector3 AmbientLightColor { get; set; }
        public List<CubeShaderParameter> ShaderParamList { get; set; }
    }

    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class CubeRenderer : Renderer
    {
        CubeShader cubeShader;

        public CubeRenderer(Game game)
            : base(game)
        {
            DrawOrder = (int)DrawOrderLabel.RENDER_CUBE;
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            var shaderService = GetService<IShaderService>();
            cubeShader = shaderService.Shaders[ShaderLabel.Cube] as CubeShader;

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
            DebugSampleManager.GetInstance().BeginTimeRuler("RenderCube");
            foreach (var p in ParamList)
            {
                var param = p as CubeRenderParameter;
                var camera = CameraManager.GetInstance().Get(param.cameraLabel);

                cubeShader.Model = param.model;
                cubeShader.World = param.world;
                cubeShader.View = camera.View;
                cubeShader.Projection = camera.Proj;
                cubeShader.EyePosition = camera.Eye;

                cubeShader.DirLight0Direction = param.DirectionalLight0.Direction;
                cubeShader.DirLight0DiffuseColor = param.DirectionalLight0.DiffuseColor;
                cubeShader.DirLight0SpecularColor = param.DirectionalLight0.SpecularColor;
                cubeShader.AmbientLightColor = param.AmbientLightColor;

                cubeShader.ParamList = param.ShaderParamList;

                cubeShader.Render();
            }
            DebugSampleManager.GetInstance().EndTimeRuler("RenderCube");

            ParamList.Clear();

            base.Draw(gameTime);
        }
    }
}
