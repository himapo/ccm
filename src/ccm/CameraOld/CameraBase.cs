using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm.CameraOld
{
    class CameraBase : MyGameComponent, ICamera
    {
        public Camera Camera { get; set; }

        public CameraBase(Game game)
            : base(game)
        {
            UpdateOrder = (int)UpdateOrderLabel.CAMERA;
            Camera = new Camera();
        }
    }
}
