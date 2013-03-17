using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Collision
{
    public class CollisionInfo
    {
        // コリジョンオブジェクト一個に一意のID
        public int ID { get; set; }

        // 現在コリジョンが有効かどうか
        public Func<bool> Active { get; set; }

        // コリジョン形状
        public List<ICollisionPrimitive> Primitives { get; private set; }

        // コリジョングループ
        public Func<int> Group { get; set; }

        // 応答前処理
        //public Action<int, int> PreReaction { get; set; }

        // 応答
        //public Action<int, int> Reaction { get; set; }

        // 応答
        public ICollisionReactor Reactor { get; set; }

        // 相手の応答に渡す自分のパラメータ
        public ICollisionActor Actor { get; set; }

        public CollisionInfo()
        {
            Primitives = new List<ICollisionPrimitive>();
            Reactor = new NullCollisionReactor();
        }
    }
}
