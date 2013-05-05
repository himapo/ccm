using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Collision;

namespace ccm.Collision
{
    public class AttackCollisionActor : ICollisionActor
    {
        /// <summary>
        /// 攻撃力
        /// </summary>
        public int Power { get; set; }

        /// <summary>
        /// よろけ倍率
        /// </summary>
        public float Shock { get; set; }
    }
}
