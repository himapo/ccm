using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ccm.CameraOld;


namespace ccm
{
    class MapCubeRenderParameter : RenderParameter
    {
        public Matrix[] WorldMatrices { get; set; }
        public Matrix[] ModelBones { get; set; }
        public DirectionalLight DirectionalLight0 { get; set; }
        public Vector3 AmbientLightColor { get; set; }
    }

    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class MapCubeRenderer : Renderer
    {
        InstancingPhongShader shader;

        public MapCubeRenderer(Game game)
            : base(game)
        {
            DrawOrder = (int)DrawOrderLabel.RENDER_MAP_CUBE;
        }

        public override void Initialize()
        {
            shader = GetService<IShaderService>().Shaders[ShaderLabel.InstancingPhong] as InstancingPhongShader;
        }

        public override void Draw(GameTime gameTime)
        {
            DebugSampleManager.GetInstance().BeginTimeRuler("RenderMapCube");
            foreach (var p in ParamList)
            {
                var param = p as MapCubeRenderParameter;
                var camera = CameraManager.GetInstance().Get(param.cameraLabel);

                shader.Model = param.model;
                shader.ModelBones = param.ModelBones;
                shader.InstanceTransforms = param.WorldMatrices;

                shader.View = camera.View;
                shader.Projection = camera.Proj;
                shader.EyePosition = camera.Eye;

                shader.DirLight0Direction = param.DirectionalLight0.Direction;
                shader.DirLight0DiffuseColor = param.DirectionalLight0.DiffuseColor;
                shader.DirLight0SpecularColor = param.DirectionalLight0.SpecularColor;
                shader.AmbientLightColor = param.AmbientLightColor;

                shader.Render();
            }
            DebugSampleManager.GetInstance().EndTimeRuler("RenderMapCube");

            ParamList.Clear();
        }
    }
}
