using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class FrameCacheData : FrameCacheDataBase
    {
        Microsoft.Xna.Framework.Matrix[] InstanceTransforms;
        
        bool InstanceTransformsUpdated = false;

        public int InstanceTransformsLength { get; private set; }

        public new static FrameCacheData Instance
        {
            get
            {
                return FrameCacheDataBase.Instance as FrameCacheData;
            }
        }

        public static void Create()
        {
            FrameCacheDataBase.instance = new FrameCacheData();   
        }

        FrameCacheData()
        {
        }

        public override void Clear()
        {
            InstanceTransformsUpdated = false;
        }

        public Microsoft.Xna.Framework.Matrix[] InstanceTransformsToArray(IEnumerable<HimaLib.Math.Matrix> transforms)
        {
            if (!InstanceTransformsUpdated)
            {
                // インスタンス化はここの1回だけにする
                var array = transforms.ToArray();

                var newCount = array.Length;

                if (InstanceTransforms == null || newCount > InstanceTransforms.Length)
                {
                    // 2倍ずつ伸張したほうがいい？
                    Array.Resize(ref InstanceTransforms, newCount);
                }

                for (var i = 0; i < newCount; ++i)
                {
                    InstanceTransforms[i] = MathUtilXna.ToXnaMatrix(array[i]);
                }

                InstanceTransformsLength = newCount;

                InstanceTransformsUpdated = true;
            }

            return InstanceTransforms;
        }
    }
}
