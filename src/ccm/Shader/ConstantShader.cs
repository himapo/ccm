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
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class ConstantShader : Shader
    {
        Effect effect;

        public Model Model { get; set; }

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public float Alpha { get; set; }
        public Texture2D DiffuseMap { get; set; }

        public Vector2 RectOffset { get; set; }
        public Vector2 RectSize { get; set; }

        VertexPositionTexture[] vertices;
        short[] indices;

        public ConstantShader(Game game)
            : base(game)
        {
            vertices = new VertexPositionTexture[4];
            indices = new short[6];

            // TODO: ここで子コンポーネントを作成します。
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {         
            // TODO: ここに初期化のコードを追加します。
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            indices[3] = 2;
            indices[4] = 1;
            indices[5] = 3;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = ResourceManager.GetInstance().Load<Effect>("Effect/Constant");
            
            base.LoadContent();
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

        public void RenderBillboard()
        {
            // ビルボードの大きさとUVを設定
            var half = 0.5f;
            vertices[0].Position = new Vector3(-RectSize.X * 0.5f - half, -RectSize.Y * 0.5f - half, 0.5f);
            vertices[1].Position = new Vector3(-RectSize.X * 0.5f - half,  RectSize.Y * 0.5f - half, 0.5f);
            vertices[2].Position = new Vector3( RectSize.X * 0.5f - half, -RectSize.Y * 0.5f - half, 0.5f);
            vertices[3].Position = new Vector3( RectSize.X * 0.5f - half,  RectSize.Y * 0.5f - half, 0.5f);

            var uvLeft = RectOffset.X / DiffuseMap.Width;
            var uvRight = (RectOffset.X + RectSize.X) / DiffuseMap.Width;
            var uvTop = RectOffset.Y / DiffuseMap.Height;
            var uvBottom = (RectOffset.Y + RectSize.Y) / DiffuseMap.Height;
            vertices[0].TextureCoordinate = new Vector2(uvLeft, uvBottom);
            vertices[1].TextureCoordinate = new Vector2(uvLeft, uvTop);
            vertices[2].TextureCoordinate = new Vector2(uvRight, uvBottom);
            vertices[3].TextureCoordinate = new Vector2(uvRight, uvTop);

            effect.Parameters["Alpha"].SetValue(Alpha);
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
            effect.Parameters["DiffuseMap"].SetValue(DiffuseMap);

            effect.CurrentTechnique = effect.Techniques["TechniqueTx"];

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Game.GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>(
                    PrimitiveType.TriangleList,
                    vertices, 0, 4,
                    indices, 0, 2);
            }
        }

        public void RenderModel()
        {
        }
    }
}
