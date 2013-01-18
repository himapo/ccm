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
        public List<ICollisionPrimitive> Primitives { get; set; }

        // コリジョングループ
        public Func<int> Group { get; set; }

        // 応答
        public Action<int, int> Reaction { get; set; }
    }
}
