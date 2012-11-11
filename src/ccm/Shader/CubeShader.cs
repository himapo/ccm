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
    enum CubeShaderType : int
    {
        Lambert,
        Phong,
    }

    class CubeShaderParameter
    {
        public CubeShaderType ShaderType { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public float Alpha { get; set; }
        public Vector3 EmissiveColor { get; set; }
        public Vector3 SpecularColor { get; set; }
        public float SpecularPower { get; set; }
    }

    /// <summary>
    /// きゅーぶシェーダ
    /// ステータスによってマテリアルの値が変わる
    /// スタックフォーメーションでは６面それぞれ異なるマテリアルになる
    /// </summary>
    class CubeShader : Shader
    {
        List<Effect> lambertList; // ６面分用意する必要がある
        List<Effect> phongList; // ６面分用意する必要がある

        public Model Model { get; set; }

        public Matrix World { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public Vector3 AmbientLightColor { get; set; }
        public Vector3 DirLight0Direction { get; set; }
        public Vector3 DirLight0DiffuseColor { get; set; }
        public Vector3 DirLight0SpecularColor { get; set; }

        public Vector3 EyePosition { get; set; }

        public List<CubeShaderParameter> ParamList { get; set; }

        public CubeShader(Game game)
            : base(game)
        {
            lambertList = new List<Effect>();
            phongList = new List<Effect>();
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
            lambertList.Add(ResourceManager.GetInstance().Load<Effect>("Effect/HalfLambert"));
            phongList.Add(ResourceManager.GetInstance().Load<Effect>("Effect/Phong"));

            for (var i = 0; i < 5; ++i)
            {
                lambertList.Add(lambertList[0].Clone());
                phongList.Add(phongList[0].Clone());
            }

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
            foreach (var mesh in Model.Meshes)
            {
                var materialCount = 0;
                foreach (var part in mesh.MeshParts)
                {
                    var param = ParamList[materialCount];

                    // シェーダを差し替え

                    switch (param.ShaderType)
                    {
                        case CubeShaderType.Lambert:
                            if (materialCount < lambertList.Count)
                            {
                                part.Effect = lambertList[materialCount];
                                part.Effect.CurrentTechnique = part.Effect.Techniques["PixelLighting"];
                            }
                            break;
                        case CubeShaderType.Phong:
                            if (materialCount < phongList.Count)
                            {
                                part.Effect = phongList[materialCount];
                                part.Effect.CurrentTechnique = part.Effect.Techniques["PixelLighting"];
                                part.Effect.Parameters["EyePosition"].SetValue(EyePosition);
                            }
                            break;
                    }

                    // マテリアルパラメータをオーバーライド
                    part.Effect.Parameters["DiffuseColor"].SetValue(param.DiffuseColor);
                    part.Effect.Parameters["Alpha"].SetValue(param.Alpha);
                    part.Effect.Parameters["EmissiveColor"].SetValue(param.EmissiveColor);
                    part.Effect.Parameters["SpecularColor"].SetValue(param.SpecularColor);
                    part.Effect.Parameters["SpecularPower"].SetValue(param.SpecularPower);

                    part.Effect.Parameters["World"].SetValue(World);
                    part.Effect.Parameters["View"].SetValue(View);
                    part.Effect.Parameters["Projection"].SetValue(Projection);

                    part.Effect.Parameters["AmbientLightColor"].SetValue(AmbientLightColor);
                    part.Effect.Parameters["DirLight0Direction"].SetValue(DirLight0Direction);
                    part.Effect.Parameters["DirLight0DiffuseColor"].SetValue(DirLight0DiffuseColor);
                    part.Effect.Parameters["DirLight0SpecularColor"].SetValue(DirLight0SpecularColor);

                    materialCount++;
                }

                mesh.Draw();
            }
        }
    }
}
