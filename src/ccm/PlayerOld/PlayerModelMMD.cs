using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MikuMikuDance.Core;
using MikuMikuDance.Core.Model;
using MikuMikuDance.Core.Motion;
using MikuMikuDance.Core.Accessory;
using MikuMikuDance.XNA.Misc;
using MikuMikuDance.XNA.Accessory;
using MikuMikuDance.XNA;

namespace ccm
{
    class PlayerModelLoadContextMMD : PlayerModelLoadContext
    {
        public Microsoft.Xna.Framework.Game Game { get; set; }
    }

    class PlayerModelUpdateContextMMD : PlayerModelUpdateContext
    {
    }

    class PlayerModelDrawContextMMD : PlayerModelDrawContext
    {
    }

    class PlayerModelMMD : PlayerModel
    {
        readonly string[] motionKeyArray = 
        {
            "stand",
            "walk",
            "run",
            "crouch",
            "attack1",
        };
        
        MMDModel model;
        
        MMDMotion motion;
        
        MMDAccessoryBase bonbon;
        MMDAccessoryBase negi;
        
        MMD_VAC bonbonVac;
        MMD_VAC negiVac;

        EdgeManager edgeManager;

        string nowMotion;
        string prevMotion;
        float shiftTime;
        float elapsedTime;

        public PlayerModelMMD()
        {
            nowMotion = "";
            prevMotion = "";
            shiftTime = 0.0f;
            elapsedTime = 0.0f;
        }

        protected override void Dispose(bool disposing)
        {
            model.Dispose();
        }

        public override void Load(PlayerModelLoadContext contextBase)
        {
            var context = contextBase as PlayerModelLoadContextMMD;
            if (context == null)
            {
                throw new ApplicationException("");
            }

            //モデルをパイプラインより読み込み
            model = MMDXCore.Instance.LoadModel("Model/petit_miku_mix2", context.Game.Content);
            //サンプルモデルはカリングを行わない。(他のモデルはカリングを行う)
            model.Culling = false;

            foreach (var motionKey in motionKeyArray)
            {
                //モーションをパイプラインより読み込み
                motion = MMDXCore.Instance.LoadMotion("Motion/" + motionKey, context.Game.Content);
                //モデルにモーションをセット
                model.AnimationPlayer.AddMotion(motionKey, motion, MMDMotionTrackOptions.UpdateWhenStopped);
            }

            // アクセサリモデルの読み込み
            bonbon = MMDXCore.Instance.LoadAccessory("Accessory/bonbon", context.Game.Content);
            negi = MMDXCore.Instance.LoadAccessory("Accessory/negi", context.Game.Content);
            // モデルとアクセサリの接続情報
            bonbonVac = MMDXCore.Instance.LoadVAC("Accessory/bonbon-vac", context.Game.Content);
            negiVac = MMDXCore.Instance.LoadVAC("Accessory/negi-vac", context.Game.Content);
            // モデルにアクセサリを持たせる
            model.BindAccessory(bonbon, bonbonVac);
            model.BindAccessory(negi, negiVac);
            // カリング設定
            bonbon.Model.Culling = true;

            //エッジマネージャの作成
            edgeManager = new EdgeManager(context.Game.Window, context.Game.GraphicsDevice);
            //エッジマネージャの登録
            MMDXCore.Instance.EdgeManager = edgeManager;
        }

        public override void Reset()
        {
            foreach (var motionKey in motionKeyArray)
            {
                model.AnimationPlayer[motionKey].BlendingFactor = 0.0f;
            }
        }

        public override void ChangeMotion(PlayerModelChangeMotionContext contextBase)
        {
            if (contextBase.MotionName == nowMotion)
                return;

            if (prevMotion != "")
                model.AnimationPlayer[prevMotion].BlendingFactor = 0.0f;

            prevMotion = nowMotion;
            nowMotion = contextBase.MotionName;
            shiftTime = contextBase.ShiftTime;
            elapsedTime = 0.0f;

            //再生した後ならリセットをかける
            if (model.AnimationPlayer[nowMotion].NowFrame > 0)
            {
                //停止
                model.AnimationPlayer[nowMotion].Stop();
                //巻き戻し
                model.AnimationPlayer[nowMotion].Reset();
                //剛体位置のリセット
                //model.PhysicsManager.Reset();
            }
            //モーションの再生
            model.AnimationPlayer[nowMotion].Start(true);
        }

        public override void Update(PlayerModelUpdateContext contextBase)
        {
            model.Transform = Transform;

            elapsedTime += (float)contextBase.GameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime = MathHelper.Clamp(elapsedTime, 0.0f, shiftTime);

            model.AnimationPlayer[nowMotion].BlendingFactor = elapsedTime / shiftTime;
            if (prevMotion != "")
            {
                model.AnimationPlayer[prevMotion].BlendingFactor = 1.0f - elapsedTime / shiftTime;
            }
        }

        public override void Draw(PlayerModelDrawContext contextBase)
        {
            //エッジ検出モードの開始
            //edgeManager.StartEdgeDetection(GraphicsDevice);
            //モデルのエッジ検出
            //model.Draw();
            //アクセサリにはエッジは出さないが、エッジの前に来る場合があるのでエッジ検出モードで描画する必要がある
            //bonbon.Draw();
            //negi.Draw();
            //エッジ検出モードの終了
            //edgeManager.EndEdgeDetection(GraphicsDevice);

            //モデルを描画
            model.Draw();
            //アクセサリを描画
            bonbon.Draw();
            negi.Draw();

            //エッジを描画
            //edgeManager.DrawEdge(GraphicsDevice);
        }
    }
}
