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
        /// よろけ値
        /// </summary>
        public int Shock { get; set; }
    }
}
