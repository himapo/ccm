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
    class ParticleManager : MyGameComponent, IParticleService
    {
        readonly string[] scriptClassArray =
        {
            "ccm.Particle_Prototype",
        };

        List<int> deadIDList;   // 死んでいる（出現させられる）IDリスト
        List<int> aliveIDList;  // 生きているIDリスト
        List<ParticleOld> particleList;

        public int InstanceNum { get; private set; }

        public int AliveNum
        {
            get { return aliveIDList.Count; }
        }

        public int DeadNum
        {
            get { return deadIDList.Count; }
        }

        public ParticleManager(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.PARTICLE;
            game.Services.AddService(typeof(IParticleService), this);

            deadIDList = new List<int>();
            aliveIDList = new List<int>();
            particleList = new List<ParticleOld>();
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
            var scriptName = "ParticleScript.cs";
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
            particleList.ForEach(x => x.Update(gameTime));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            particleList.ForEach(x => x.Draw(gameTime));

            base.Draw(gameTime);
        }

        public void Add(ParticleInfo info)
        {
            info.ScriptClass = scriptClassArray[(int)info.Type];

            if (deadIDList.Count > 0)
            {
                var id = deadIDList[0];
                deadIDList.Remove(id);
                aliveIDList.Add(id);
                particleList[id].Appear(info);
            }
            else
            {
                var particle = new ParticleOld(Game);
                particle.ID = InstanceNum++;
                aliveIDList.Add(particle.ID);
                particleList.Add(particle);
                particle.Appear(info);
            }
        }

        public void Remove(int ID)
        {
            particleList[ID].Disappear();
            deadIDList.Add(ID);
            aliveIDList.Remove(ID);
        }
    }
}
