using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// 参考用のプロトタイプ
    /// </summary>
    public class Deco_Prototype : Deco_Common
    {
        public static void Appear(Game game, Deco myself)
        {
            // パーティクル生成
            AddParticle(
                game,
                myself,
                new ParticleInfo
                {
                    Type = ParticleLabel.Prototype,
                    BasePosition = myself.Position,
                    DecoID = myself.ID,
                    ScriptClass = ""
                }
            );
        }

        public static void Update(GameTime gameTime, Game game, Deco myself)
        {
        }
    }

    /// <summary>
    /// スクリプトに共通の関数をまとめたクラス
    /// </summary>
    public class Deco_Common
    {
        // サービス取得
        protected static ServiceType GetService<ServiceType>(Game game)
        {
            return (ServiceType)game.Services.GetService(typeof(ServiceType));
        }

        // パーティクル生成
        protected static void AddParticle(Game game, Deco myself, ParticleInfo info)
        {
            GetService<IParticleService>(game).Add(info);
            myself.ParticleNum++;
        }
    }
}
