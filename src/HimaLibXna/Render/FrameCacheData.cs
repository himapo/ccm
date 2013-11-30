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
        InstanceTransforms InstanceTransforms = new InstanceTransforms();

        bool InstanceTransformsUpdated = false;

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

        public InstanceTransforms InstanceTransformsToArray(IEnumerable<HimaLib.Math.Matrix> transforms)
        {
            if (!InstanceTransformsUpdated)
            {
                InstanceTransforms.Update(transforms);

                InstanceTransformsUpdated = true;
            }

            return InstanceTransforms;
        }
    }
}
