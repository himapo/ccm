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
    class DebugAxis : MyGameComponent
    {
        BasicEffect basicEffect;
        VertexPositionColor[] vertices;

        CameraLabel cameraLabel;

        public DebugAxis(Game game, CameraLabel cameraLabel)
            : base(game)
        {
            this.cameraLabel = cameraLabel;
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            basicEffect = new BasicEffect(Game.GraphicsDevice);
            basicEffect.VertexColorEnabled = true;
            basicEffect.View = Matrix.CreateLookAt(new Vector3(0, 0, 800.0f), Vector3.Zero, Vector3.Up);
            basicEffect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(GameProperty.fov),
                (float)GameProperty.resolutionWidth / GameProperty.resolutionHeight,
                10.0f,
                1000.0f);

            vertices = new VertexPositionColor[804];
            // 座標とカラーを持った頂点を設定
            for (int i = 0; i < 201; i++)
            {
                vertices[i * 4 + 0].Position = new Vector3(-400.0f, 0.0f, -400.0f + 4.0f * i);
                vertices[i * 4 + 1].Position = new Vector3(400.0f, 0.0f, -400.0f + 4.0f * i);
                vertices[i * 4 + 2].Position = new Vector3(-400.0f + 4.0f * i, 0.0f, -400.0f);
                vertices[i * 4 + 3].Position = new Vector3(-400.0f + 4.0f * i, 0.0f, 400.0f);
                vertices[i * 4 + 0].Color = Color.Black;
                vertices[i * 4 + 1].Color = Color.Black;
                vertices[i * 4 + 2].Color = Color.Black;
                vertices[i * 4 + 3].Color = Color.Black;
            }
            vertices[400].Color = Color.Red;
            vertices[401].Color = Color.Red;
            vertices[402].Color = Color.Blue;
            vertices[403].Color = Color.Blue;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            var camera = CameraManager.GetInstance().Get(cameraLabel);

            basicEffect.View = camera.View;
            basicEffect.Projection = camera.Proj;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //basicEffect.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                Game.GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.LineList, vertices, 0, 402);
            }

            base.Draw(gameTime);
        }
    }
}
