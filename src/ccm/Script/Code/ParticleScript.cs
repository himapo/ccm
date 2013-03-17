using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ccm
{
    /// <summary>
    /// 参考用のプロトタイプ
    /// </summary>
    public class Particle_Prototype : Particle_Common
    {
        public static void Appear(Game game, ParticleOld myself, Vector3 basePosition)
        {
            // パラメータ設定
            myself.DiffuseMap = ResourceManager.GetInstance().Load<Texture2D>("Texture/miki");
            myself.DiffuseMapOffset = new Vector2(0.0f, 0.0f);
            myself.DiffuseMapSize = new Vector2(256.0f, 256.0f);
            myself.Scale = 0.01f;
            myself.Alpha = 1.0f;

            // 円柱アップデータで回りながら上昇
            new CylinderUpdater(
                GetService<IUpdaterService>(game),
                2000.0f,
                0.0f,
                720.0f,
                new Vector2(basePosition.X, basePosition.Z),
                new Vector2(5.0f, 5.0f),
                a => { myself.Position.X = a; },
                a => { myself.Position.Z = a; },
                basePosition.Y,
                basePosition.Y + 15.0f,
                a => { myself.Position.Y = a; },
                () => { });

            // アルファを線形アップデータで減衰
            new LinearUpdater(
                GetService<IUpdaterService>(game),
                2000.0f,
                1.0f,
                0.0f,
                a => { myself.Alpha = a; },
                () => { myself.KillMe(); });
        }

        public static void Update(Game game, ParticleOld myself, GameTime gameTime)
        {
        }
    }

    /// <summary>
    /// スクリプトに共通の関数をまとめたクラス
    /// </summary>
    public class Particle_Common
    {
        // サービス取得
        protected static ServiceType GetService<ServiceType>(Game game)
        {
            return (ServiceType)game.Services.GetService(typeof(ServiceType));
        }
    }
}
