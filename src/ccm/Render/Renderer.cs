using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ccm
{
    enum RendererLabel
    {
        UI,
        Billboard,
        CubeBasic,
        Cube,
        Miku,
        MapCube,
    }

    /// <summary>
    /// 共通のレンダリングパラメータ
    /// </summary>
    class RenderParameter
    {
        public RendererLabel renderer = RendererLabel.CubeBasic;
        public CameraLabel cameraLabel = CameraLabel.Game;
        public bool cullEnable = true;
        public bool isCulled = false;
        public Model model = null;
        public Matrix world = Matrix.Identity;
    }

    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class Renderer : MyGameComponent
    {
        public List<RenderParameter> ParamList { get; private set; }

        public Renderer(Game game)
            : base(game)
        {
            ParamList = new List<RenderParameter>();
        }
    }
}
