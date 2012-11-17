using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.System;
using HimaLib.Render;
using HimaLib.Math;

namespace ccm.Scene
{
    public class BootScene : SceneBase
    {
        UIBillboardRenderer renderer;

        public BootScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;
        }

        void UpdateStateInit()
        {
            // コンストラクタではContentの初期化ができてないのでここで
            renderer = new UIBillboardRenderer();

            renderer.TextureName = "Texture/miki";
            renderer.Alpha = 1.0f;
            renderer.Scale = 1.0f;
            renderer.Rotation = new Vector3(0.0f);
            renderer.Position = new Vector3(0.0f);

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
        }

        void DrawStateMain()
        {
            renderer.Render();
        }
    }
}
