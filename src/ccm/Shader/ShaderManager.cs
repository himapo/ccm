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
    class ShaderManager : MyGameComponent, IShaderService
    {
        public Dictionary<ShaderLabel, Shader> Shaders { get; set; }

        public ShaderManager(Game game)
            : base(game)
        {
            Shaders = new Dictionary<ShaderLabel, Shader>();

            game.Services.AddService(typeof(IShaderService), this);

            AddShader(ShaderLabel.Constant, new ConstantShader(game));
            AddShader(ShaderLabel.Lambert, new LambertShader(game));
            AddShader(ShaderLabel.Phong, new PhongShader(game));
            AddShader(ShaderLabel.Cube, new CubeShader(game));
            AddShader(ShaderLabel.InstancingPhong, new InstancingPhongShader(game));

            AddComponents();
        }

        void AddShader(ShaderLabel label, Shader shader)
        {
            ChildComponents.Add(shader);
            Shaders[label] = shader;
        }
    }
}
