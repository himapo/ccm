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
    public class DecoOld : MyGameComponent
    {
        enum State
        {
            Alive,
            Dead
        }

        public int ID { get; set; }

        public DecoLabel Type { get; private set; }

        Vector3 position;
        public Vector3 Position { get { return position; } set { position = value; } }

        public int ParticleNum { get; set; }

        State state;
        Action<GameTime> updateFunc;
        Action<GameTime> drawFunc;
        string scriptName;
        string scriptClass;

        public DecoOld(Game game)
            : base(game)
        {
            scriptName = "DecoScript.cs";
            ParticleNum = 0;

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
                script.Call(scriptClass, "Update", new object[] { gameTime, Game, this });
            }
            else
            {
                DebugUtil.PrintLine("スクリプト {0} がありません", scriptName);
                KillMe();
            }
        }

        void DrawAlive(GameTime gameTime)
        {
        }

        void UpdateDead(GameTime gameTime)
        {
        }

        void DrawDead(GameTime gameTime)
        {
        }

        // 出現
        public void Appear(DecoInfo info)
        {
            Type = info.Type;
            Position = info.Position;
            scriptClass = info.ScriptClass;
            ParticleNum = 0;

            // スクリプト呼び出し
            var scriptService = GetService<IScriptService>();
            var script = scriptService.Get(scriptName);
            if (script != null)
            {
                DebugUtil.PrintLine("Deco {0} appears.", ID);
                script.Call(scriptClass, "Appear", new object[] { Game, this });
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
            DebugUtil.PrintLine("Deco {0} disappears.", ID);
        }

        // 自殺
        public void KillMe()
        {
            // マネージャに消してもらう
            GetService<IDecoService>().Remove(ID);
        }

        // パーティクル終了通知
        public void NotifyFinishParticle()
        {
            ParticleNum--;
            if (ParticleNum == 0)
            {
                // デコ終了
                KillMe();
            }
            else if (ParticleNum < 0)
            {
                DebugUtil.PrintLine("Warning : 不正なパーティクル終了通知");
            }
        }
    }
}
