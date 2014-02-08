using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.System
{
    /// <summary>
    /// 描画オプション制御XNA実装
    /// </summary>
    public class GraphicsOption : GraphicsOptionBase
    {
        XnaGame XnaGame { get { return XnaGame.Instance; } }

        public new static GraphicsOption Instance
        {
            get { return GraphicsOptionBase.Instance as GraphicsOption; }
        }

        public static void Create()
        {
            GraphicsOptionBase.instance = new GraphicsOption();
        }

        GraphicsOption()
        {
        }

        public override void GetCurrentSetting()
        {
            VSyncEnable = XnaGame.VSyncEnable;
        }

        public override void Apply()
        {
            XnaGame.VSyncEnable = VSyncEnable;

            XnaGame.ApplyGraphicsChangesFlag = true;
        }
    }
}
