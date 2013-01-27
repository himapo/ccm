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
    class EnemyManager : MyGameComponent, IEnemyService
    {
        public static readonly int ENEMY_MAX = 20;

        List<EnemyCube> enemyCubeList;

        public EnemyManager(Game game)
            : base(game)
        {
            game.Services.AddService(typeof(IEnemyService), this);

            enemyCubeList = new List<EnemyCube>();

            // TODO: ここで子コンポーネントを作成します。
            for (var i = 0; i < ENEMY_MAX; ++i)
            {
                var enemy = new EnemyCube(game);
                enemyCubeList.Add(enemy);
                ChildComponents.Add(enemy);
            }

            AddComponents();
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

            base.Update(gameTime);
        }

        public void Add()
        {
            foreach (var enemy in enemyCubeList)
            {
                if (enemy.NowState == EnemyCube.State.Empty)
                {
                    enemy.Appear();
                    break;
                }
            }
        }

        public void Clear()
        {
            foreach (var enemy in enemyCubeList)
            {
                enemy.Disappear();
            }
        }

        public void DamageRange(int damage, Vector3 source, float range)
        {
            foreach (var enemy in enemyCubeList)
            {
                if ((enemy.Position - source).Length() < range)
                {
                    enemy.Damage(damage);
                }
            }
        }
    }
}
