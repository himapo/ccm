using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Input;
using HimaLib.Math;
using ccm.Input;

namespace ccm.Camera
{
    public class TPSCameraUpdater : ViewerCameraUpdater
    {
        public TPSCameraUpdater(CameraBase camera, IController controller)
            : base(camera, controller)
        {
            EnablePan = false;
            RotWithoutKey = true;
        }
    }
}
