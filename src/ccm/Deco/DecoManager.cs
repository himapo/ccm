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
    class DecoManager : MyGameComponent, IDecoService
    {
        readonly string[] scriptClassArray =
        {
            "ccm.Deco_Prototype",
        };

        List<int> deadIDList;   // 死んでいる（出現させられる）IDリスト
        List<int> aliveIDList;  // 生きているIDリスト
        List<Deco> decoList;

        public int InstanceNum { get; private set; }

        public int AliveNum
        {
            get { return aliveIDList.Count; }
        }

        public int DeadNum
        {
            get { return deadIDList.Count; }
        }

        public DecoManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.DECO;
            game.Services.AddService(typeof(IDecoService), this);

            deadIDList = new List<int>();
            aliveIDList = new List<int>();
            decoList = new List<Deco>();
            InstanceNum = 0;

            // TODO: ここで子コンポーネントを作成します。
        }

        /// <summary>
        /// ゲーム コンポーネントの初期化を行います。
        /// ここで、必要なサービスを照会して、使用するコンテンツを読み込むことができます。
        /// </summary>
        public override void Initialize()
        {
            // TODO: ここに初期化のコードを追加します。
            var scriptService = GetService<IScriptService>();
            var scriptName = "DecoScript.cs";
            if (!scriptService.Load(scriptName))
            {
                Console.WriteLine("スクリプト読み込み失敗 : " + scriptName);
            }

            base.Initialize();
        }

        /// <summary>
        /// ゲーム コンポーネントが自身を更新するためのメソッドです。
        /// </summary>
        /// <param name="gameTime">ゲームの瞬間的なタイミング情報</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: ここにアップデートのコードを追加します。
            decoList.ForEach(x => x.Update(gameTime));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            decoList.ForEach(x => x.Draw(gameTime));

            base.Draw(gameTime);
        }

        public void Add(DecoInfo info)
        {
            info.ScriptClass = scriptClassArray[(int)info.Type];

            if (deadIDList.Count > 0)
            {
                var id = deadIDList[0];
                deadIDList.Remove(id);
                aliveIDList.Add(id);
                decoList[id].Appear(info);
            }
            else
            {
                var deco = new Deco(Game);
                deco.ID = InstanceNum++;
                aliveIDList.Add(deco.ID);
                decoList.Add(deco);
                deco.Appear(info);
            }
        }

        public void Remove(int ID)
        {
            decoList[ID].Disappear();
            deadIDList.Add(ID);
            aliveIDList.Remove(ID);
        }

        public void NotifyFinishParticle(int decoID, int particleID)
        {
            if (deadIDList.Contains(decoID))
            {
                DebugUtil.PrintLine("Warning : すでに消えたデコのパーティクル死亡通知が来てる");
                return;
            }

            decoList[decoID].NotifyFinishParticle();
        }
    }
}
