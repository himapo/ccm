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
    class UpdaterManager : MyGameComponent, IUpdaterService
    {
        List<IUpdater> updaterList;

        public UpdaterManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.UPDATER;
            game.Services.AddService(typeof(IUpdaterService), this);

            updaterList = new List<IUpdater>();

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

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。
            var elapsedMilliSeconds = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            updaterList.ForEach(x => x.Update(elapsedMilliSeconds));

            updaterList.RemoveAll(x => x.Finish);

            base.Update(gameTime);
        }

        public void Add(IUpdater updater)
        {
            updaterList.Add(updater);
        }
    }
}
