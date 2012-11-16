using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm.CameraOld
{
    enum CameraLabel
    {
        Title,
        Game,
        UI,

        // for debug
        ModelViewer,
        MapViewer,
    }

    class CameraManager : MyGameComponent
    {
        static CameraManager instance;

        Dictionary<CameraLabel, CameraBase> cameraDic;

        public static void CreateInstance(Game game)
        {
            instance = new CameraManager(game);
        }

        public static CameraManager GetInstance()
        {
            return instance;
        }

        CameraManager(Game game)
            : base(game)
        {
            cameraDic = new Dictionary<CameraLabel, CameraBase>();
            
            // カメラ生成
            AddCamera(CameraLabel.Game, new ManualCamera(game));
            AddCamera(CameraLabel.UI, new UICamera(game));
            AddCamera(CameraLabel.ModelViewer, new ModelViewerCamera(game));
            AddCamera(CameraLabel.MapViewer, new FreeLookCamera(game));

            AddComponents();
        }

        void AddCamera(CameraLabel label, CameraBase camera)
        {
            ChildComponents.Add(camera);
            cameraDic[label] = camera;
        }

        public Camera Get(CameraLabel label)
        {
            CameraBase camera;
            cameraDic.TryGetValue(label, out camera);
            return camera.Camera;
        }
    }
}
