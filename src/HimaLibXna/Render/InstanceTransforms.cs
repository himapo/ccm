using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using HimaLib.Math;

namespace HimaLib.Render
{
    /// <summary>
    /// 実際の配列のサイズと使用するサイズが異なるので
    /// 間違えないようにカプセル化
    /// </summary>
    public class InstanceTransforms
    {
        public int Length { get; private set; }

        Microsoft.Xna.Framework.Matrix[] matrices;
        public Microsoft.Xna.Framework.Matrix[] Matrices { get { return matrices; } }

        public InstanceTransforms()
        {
        }

        public void Update(IEnumerable<HimaLib.Math.Matrix> transforms)
        {
            // インスタンス化はここの1回だけにする
            var array = transforms.ToArray();

            var newCount = array.Length;

            if (matrices == null || newCount > matrices.Length)
            {
                // 2倍ずつ伸張したほうがいい？
                Array.Resize(ref matrices, newCount);
            }

            for (var i = 0; i < newCount; ++i)
            {
                matrices[i] = MathUtilXna.ToXnaMatrix(array[i]);
            }

            Length = newCount;
        }
    }
}
