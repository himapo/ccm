using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    /// <summary>
    /// 描画オプション制御
    /// </summary>
    public abstract class GraphicsOptionBase
    {
        protected static GraphicsOptionBase instance;
        public static GraphicsOptionBase Instance { get { return instance; } }

        public bool VSyncEnable { get; set; }

        public abstract void GetCurrentSetting();

        public abstract void Apply();
    }
}
