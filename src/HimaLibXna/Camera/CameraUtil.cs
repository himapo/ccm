using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using HimaLib.Math;

namespace HimaLib.Camera
{
    public static class CameraUtil
    {
        public static Microsoft.Xna.Framework.Matrix GetViewMatrix(ICamera camera)
        {
            return MathUtilXna.ToXnaMatrix(HimaLib.Math.Matrix.CreateLookAt(camera.Eye, camera.At, camera.Up));
        }

        public static Microsoft.Xna.Framework.Matrix GetProjMatrix(ICamera camera)
        {
            return MathUtilXna.ToXnaMatrix(HimaLib.Math.Matrix.CreatePerspectiveFieldOfView(MathUtil.ToRadians(camera.FovY), camera.Aspect, camera.Near, camera.Far));
        }
    }
}
