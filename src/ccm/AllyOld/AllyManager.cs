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
    class AllyManager : MyGameComponent, IAllyService
    {
        public static readonly int ALLY_MAX = 6;

        List<AllyCube> allyCubeList;

        public AllyManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IAllyService), this);

            allyCubeList = new List<AllyCube>();

            // TODO: ここで子コンポーネントを作成します。
            for (var i = 0; i < ALLY_MAX; ++i)
            {
                var ally = new AllyCube(game);
                allyCubeList.Add(ally);
                ChildComponents.Add(ally);
            }

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

        public override void OnSceneBegin(SceneLabel sceneLabel)
        {
            if (sceneLabel == SceneLabel.GAME_SCENE)
            {
                Enabled = true;
                Visible = true;
            }
            else
            {
                Enabled = false;
                Visible = false;
            }
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
    }
}