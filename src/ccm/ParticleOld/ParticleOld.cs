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
using ccm.CameraOld;


namespace ccm
{
    /// <summary>
    /// IUpdateable インターフェイスを実装したゲーム コンポーネントです。
    /// </summary>
    public class ParticleOld : MyGameComponent
    {
        enum State
        {
            Alive,
            Dead
        }

        public int ID { get; set; }
        public int DecoID { get; set; }

        public ParticleLabel Type { get; private set; }

        // パーティクル自体の位置
        public Vector3 Position;

        // スケール
        public float Scale;

        // アルファ
        public float Alpha { get; set; }

        // テクスチャ
        public Texture2D DiffuseMap { get; set; }
        public Vector2 DiffuseMapOffset { get; set; }
        public Vector2 DiffuseMapSize { get; set; }

        State state;
        Action<GameTime> updateFunc;
        Action<GameTime> drawFunc;
        string scriptName;
        string scriptClass;

        public ParticleOld(Game game)
            : base(game)
        {
            ID = -1;
            DecoID = -1;
            Position = new Vector3();
            Scale = 1.0f;
            Alpha = 1.0f;
            scriptName = "ParticleScript.cs";

            // TODO: ここで子コンポーネントを作成します。

            SetState(State.Alive);
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
            updateFunc(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            drawFunc(gameTime);

            base.Draw(gameTime);
        }

        void SetState(State state)
        {
            if (state == State.Alive)
            {
                updateFunc = UpdateAlive;
                drawFunc = DrawAlive;
            }
            else if (state == State.Dead)
            {
                updateFunc = UpdateDead;
                drawFunc = DrawDead;
            }

            this.state = state;
        }

        void UpdateAlive(GameTime gameTime)
        {
            // スクリプト呼び出し
            var scriptService = GetService<IScriptService>();
            var script = scriptService.Get(scriptName);
            if (script != null)
            {
                script.Call(scriptClass, "Update", new object[] { Game, this, gameTime });
            }
            else
            {
                DebugUtil.PrintLine("スクリプト {0} がありません", scriptName);
                KillMe();
            }
        }

        void DrawAlive(GameTime gameTime)
        {

            var renderParam = new BillboardRenderParameter();
            renderParam.cameraLabel = CameraLabel.Game;
            renderParam.DiffuseMap = DiffuseMap;
            renderParam.ScaleMatrix = Matrix.CreateScale(Scale);
            renderParam.RotatMatrix = Matrix.Identity;
            renderParam.TransMatrix = Matrix.CreateTranslation(Position);
            renderParam.renderer = RendererLabel.Billboard;
            renderParam.cullEnable = false;

            renderParam.RectOffset = DiffuseMapOffset;
            renderParam.RectSize = DiffuseMapSize;
            renderParam.Alpha = Alpha;

            RenderManager.GetInstance().Register(renderParam);
        }

        void UpdateDead(GameTime gameTime)
        {
        }

        void DrawDead(GameTime gameTime)
        {
        }

        // 出現
        public void Appear(ParticleInfo info)
        {
            DecoID = info.DecoID;
            Type = info.Type;
            scriptClass = info.ScriptClass;

            // スクリプト呼び出し
            var scriptService = GetService<IScriptService>();
            var script = scriptService.Get(scriptName);
            if (script != null)
            {
                DebugUtil.PrintLine("Particle {0} appears.", ID);
                script.Call(scriptClass, "Appear", new object[] { Game, this, info.BasePosition });
                SetState(State.Alive);
            }
            else
            {
                DebugUtil.PrintLine("スクリプト {0} がありません", scriptName);
                KillMe();
            }
        }

        // 消滅（これはマネージャからしか呼んではいけない）
        public void Disappear()
        {
            SetState(State.Dead);
            DebugUtil.PrintLine("Particle {0} disappears.", ID);
        }

        // 自殺
        public void KillMe()
        {
            // マネージャに消してもらう
            GetService<IParticleService>().Remove(ID);

            // 親に終了通知
            GetService<IDecoService>().NotifyFinishParticle(DecoID, ID);
        }
    }
}
