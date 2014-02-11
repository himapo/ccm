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

        public bool MSAAEnable { get; set; }

        public bool IsFullScreen { get; set; }

        public List<Resolution> Resolutions { get; private set; }

        public Resolution Resolution { get; set; }

        protected GraphicsOptionBase()
        {
            Resolutions = new List<Resolution>();
            Resolution = new Resolution();
        }

        public abstract void GetCurrentSetting();

        public abstract void Apply();
    }
}
