using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Render
{
    /// <summary>
    /// 1フレームの間に1度だけ作ればいいデータをキャッシュする
    /// </summary>
    public abstract class FrameCacheDataBase
    {
        protected static FrameCacheDataBase instance;
        public static FrameCacheDataBase Instance { get { return instance; } }

        public abstract void Clear();
    }
}
