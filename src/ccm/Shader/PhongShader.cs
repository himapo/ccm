using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ccm
{
    /// <summary>
    /// Phong シェーダ
    /// </summary>
    class PhongShader : Shader
    {
        Effect effect;

        public Model Model { get; set; }

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public Vector3 AmbientLightColor { get; set; }
        public Vector3 DirLight0Direction { get; set; }
        public Vector3 DirLight0DiffuseColor { get; set; }
        public Vector3 DirLight0SpecularColor { get; set; }

        public Vector3 EyePosition { get; set; }

        public PhongShader(Game game)
            : base(game)
        {
            // TODO: ここで子コンポーネントを作成します。
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。

            base.Initialize();
        }

        protected override void LoadContent()
        {
            effect = ResourceManager.GetInstance().Load<Effect>("Effect/Phong");

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

        public void Render()
        {
            var newEffects = new Dictionary<string, Effect>();

            foreach (var mesh in Model.Meshes)
            {
                foreach (var meshEffect in mesh.Effects)
                {
                    var basicEffect = meshEffect as BasicEffect;

                    if (basicEffect != null)
                    {
                        // カスタムエフェクトに差し替える
                        var newEffect = effect.Clone();
                        newEffect.CurrentTechnique = newEffect.Techniques["PixelLighting"];

                        // マテリアルをコピー
                        newEffect.Parameters["DiffuseColor"].SetValue(basicEffect.DiffuseColor);
                        newEffect.Parameters["Alpha"].SetValue(basicEffect.Alpha);
                        newEffect.Parameters["EmissiveColor"].SetValue(basicEffect.EmissiveColor);
                        newEffect.Parameters["SpecularColor"].SetValue(basicEffect.SpecularColor);
                        newEffect.Parameters["SpecularPower"].SetValue(basicEffect.SpecularPower);

                        newEffects[basicEffect.Name] = newEffect;
                    }
                }

                foreach (var part in mesh.MeshParts)
                {
                    if (newEffects.ContainsKey(part.Effect.Name))
                    {
                        part.Effect = newEffects[part.Effect.Name];
                    }

                    part.Effect.Parameters["World"].SetValue(World);
                    part.Effect.Parameters["View"].SetValue(View);
                    part.Effect.Parameters["Projection"].SetValue(Projection);

                    part.Effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);
                    part.Effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
                    part.Effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);
                    part.Effect.Parameters["DirLight0SpecularColor"].SetValue(DirLight0SpecularColor);

                    part.Effect.Parameters["EyePosition"].SetValue(EyePosition);
                }

                mesh.Draw();
            }
        }
    }
}
