using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    class RenderManager : MyGameComponent
    {
        static RenderManager instance;

        List<RenderParameter> paramList;
        int rendered = 0;

        Dictionary<RendererLabel, Renderer> Renderers { get; set; }

        public static void CreateInstance(Game game)
        {
            instance = new RenderManager(game);
        }

        public static RenderManager GetInstance()
        {
            return instance;
        }

        RenderManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.RENDER;
            DrawOrder = (int)DrawOrderLabel.RENDER;
            paramList = new List<RenderParameter>();

            Renderers = new Dictionary<RendererLabel, Renderer>();

            AddRenderer(RendererLabel.CubeBasic, new CubeBasicRenderer(game));
            AddRenderer(RendererLabel.Cube, new CubeRenderer(game));
            AddRenderer(RendererLabel.UI, new UIRenderer(game));
            AddRenderer(RendererLabel.Billboard, new BillboardRenderer(game));
            AddRenderer(RendererLabel.MapCube, new MapCubeRenderer(game));
            
            AddComponents();
        }

        void AddRenderer(RendererLabel label, Renderer renderer)
        {
            ChildComponents.Add(renderer);
            Renderers[label] = renderer;
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
            var registered = paramList.Count;
            paramList.Clear();

            // デバッグ出力
            var debugFont = DebugFontManager.GetInstance();
            var outputPos = new Vector2(900.0f, 88.0f);

            var outputString = String.Format("object num {0}", registered);
            debugFont.DrawString(new DebugFontInfo(outputString, outputPos));
            outputPos.Y += 22.0f;
            outputString = String.Format("rendered num {0}", rendered);
            debugFont.DrawString(new DebugFontInfo(outputString, outputPos));

            FrustumCulling.GetInstance().ClearFrustum();

            base.Update(gameTime);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            // 深度テスト有効
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            DebugSampleManager.GetInstance().BeginTimeRuler("CullFrustum");
            CullFrustum();
            DebugSampleManager.GetInstance().EndTimeRuler("CullFrustum");

            rendered = 0;

            foreach (var param in paramList)
            {
                if (param.isCulled)
                    continue;

                Renderers[param.renderer].ParamList.Add(param);

                rendered++;
            }
        }

        /// <summary>
        /// 描画登録
        /// </summary>
        /// <param name="parameter"></param>
        public void Register(RenderParameter param)
        {
            paramList.Add(param);
        }

        /// <summary>
        /// 視錐台カリング
        /// </summary>
        void CullFrustum()
        {
            foreach (var param in paramList)
            {
                if (!param.cullEnable)
                {
                    param.isCulled = false;
                    continue;
                }

                // デフォルトはカリング有効にしておく
                param.isCulled = FrustumCulling.GetInstance().IsCulled(param.cameraLabel, param.model, param.world);
            }
        }
    }
}
