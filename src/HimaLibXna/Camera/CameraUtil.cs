using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace HimaLib.Camera
{
    public static class CameraUtil
    {
        public static Matrix GetViewMatrix(ICamera camera)
        {
            return Matrix.CreateLookAt(
                Math.Vector3.CreateXnaVector(camera.Eye),
                Math.Vector3.CreateXnaVector(camera.At),
                Math.Vector3.CreateXnaVector(camera.Up));
        }

        public static Matrix GetProjMatrix(ICamera camera)
        {
            return Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(camera.FovY), camera.Aspect, camera.Near, camera.Far);
        }
    }
}
