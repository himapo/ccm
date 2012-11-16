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
    class BillboardRenderParameter : RenderParameter
    {
        public float Alpha { get; set; }
        public Texture2D DiffuseMap { get; set; }
        public Vector2 RectOffset { get; set; }     // テクスチャのサンプリングオフセット
        public Vector2 RectSize { get; set; }       // テクスチャのサンプリングサイズ
        public Matrix ScaleMatrix { get; set; }
        public Matrix RotatMatrix { get; set; }
        public Matrix TransMatrix { get; set; }
    }

    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class BillboardRenderer : Renderer
    {
        ConstantShader constant;

        public BillboardRenderer(Game game)
            : base(game)
        {
            DrawOrder = (int)DrawOrderLabel.RENDER_BILLBOARD;
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            var shaderService = GetService<IShaderService>();
            constant = shaderService.Shaders[ShaderLabel.Constant] as ConstantShader;

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
            // Shaderのプロパティにパラメータセット

            DebugSampleManager.GetInstance().BeginTimeRuler("RenderBillboard");
            foreach (var p in ParamList)
            {
                var param = p as BillboardRenderParameter;

                var camera = CameraManager.GetInstance().Get(param.cameraLabel);

                // 視線と逆を向く回転行列作成
                var rotatMatrix = Matrix.CreateLookAt(
                    Vector3.Zero,
                    camera.At - camera.Eye,
                    camera.Up);
                rotatMatrix = Matrix.Invert(rotatMatrix);

                constant.World = param.ScaleMatrix * param.RotatMatrix * rotatMatrix * param.TransMatrix;
                constant.View = camera.View;
                constant.Projection = camera.Proj;
                constant.Alpha = param.Alpha;
                constant.DiffuseMap = param.DiffuseMap;
                constant.RectOffset = param.RectOffset;
                constant.RectSize = param.RectSize;

                constant.RenderBillboard();
            }
            DebugSampleManager.GetInstance().EndTimeRuler("RenderBillboard");

            ParamList.Clear();

            base.Draw(gameTime);
        }
    }
}
