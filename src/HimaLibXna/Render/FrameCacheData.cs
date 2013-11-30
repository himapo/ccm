using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    public class FrameCacheData : FrameCacheDataBase
    {
        public static void Create()
        {
            FrameCacheDataBase.instance = new FrameCacheData();   
        }

        FrameCacheData()
        {
        }

        public override void Clear()
        {
            
        }
    }
}
