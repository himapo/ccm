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
    class LightManager : MyGameComponent, ILightService
    {
        List<DirectionalLight> directionalLights;
        List<PointLight> pointLights;

        public LightManager(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            directionalLights = new List<DirectionalLight>();
            pointLights = new List<PointLight>();

            game.Services.AddService(typeof(ILightService), this);

            AddComponents();
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

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。

            base.Update(gameTime);
        }

        public bool Add(DirectionalLight light)
        {
            if (directionalLights.Contains(light))
            {
                return false;
            }

            directionalLights.Add(light);

            return true;
        }

        public bool Remove(DirectionalLight light)
        {
            return directionalLights.Remove(light);
        }

        public DirectionalLight Get(LightAttribute attribute)
        {
            return directionalLights.Find((light) => { return light.Attributes.Contains(attribute); });
        }
    }
}
