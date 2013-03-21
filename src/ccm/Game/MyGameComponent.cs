using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    public class MyGameComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected List<MyGameComponent> ChildComponents { get; private set; }

        protected ServiceType GetService<ServiceType>()
        {
            return (ServiceType)Game.Services.GetService(typeof(ServiceType));
        }

        public MyGameComponent(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.DEFAULT;
            DrawOrder = (int)DrawOrderLabel.DEFAULT;

            // TODO: ここで子コンポーネントを作成します。
            ChildComponents = new List<MyGameComponent>();

            EnabledChanged += new EventHandler<EventArgs>(MyGameComponent_EnabledChanged);
            VisibleChanged += new EventHandler<EventArgs>(MyGameComponent_VisibleChanged);
        }

        void MyGameComponent_EnabledChanged(object sender, EventArgs e)
        {
            ChildComponents.ForEach((component) => component.Enabled = Enabled);
        }

        void MyGameComponent_VisibleChanged(object sender, EventArgs e)
        {
            ChildComponents.ForEach((component) => component.Visible = Visible);
        }

        /// <summary>
        /// 子コンポーネントをゲームに登録
        /// </summary>
        protected void AddComponents()
        {
            ChildComponents.ForEach((x) =>
            {
                Game.Components.Add(x);
            });
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
        /// シーン開始時に呼ばれるメソッド
        /// </summary>
        /// <param name="sceneLabel"></param>
        public virtual void OnSceneBegin(SceneLabel sceneLabel)
        {
        }

        /// <summary>
        /// シーン終了時に呼ばれるメソッド
        /// </summary>
        /// <param name="sceneLabel"></param>
        public virtual void OnSceneEnd(SceneLabel sceneLabel)
        {
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
