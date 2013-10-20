﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Camera;
using HimaLib.Math;

namespace HimaLib.Render
{
    public class FrustumCulling
    {
        /// <summary>
        /// 視錐台を覆うAABBのXYZそれぞれの最大座標
        /// </summary>
        Vector3 MinFrustumAABB;

        /// <summary>
        /// 視錐台を覆うAABBのXYZそれぞれの最小座標
        /// </summary>
        Vector3 MaxFrustumAABB;

        public void UpdateFrustum(CameraBase camera)
        {
            var invViewProj = Matrix.Invert(camera.View * camera.Projection);

            var projCorners = new Vector3[8]
            {
                new Vector3(-1, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(-1, -1, 0),
                new Vector3(1, -1, 0),
                new Vector3(-1, 1, 1),
                new Vector3(1, 1, 1),
                new Vector3(-1, -1, 1),
                new Vector3(1, -1, 1),
            };

            var worldCorners = projCorners.Select(c =>
            {
                return Vector3.TransformCoord(c, invViewProj);
            });

            // 計算を軽くするため、視錐台に外接するAABBでカリングする
            MinFrustumAABB = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            MaxFrustumAABB = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var frustumCorner in worldCorners)
            {
                MinFrustumAABB.X = MathUtil.Min(MinFrustumAABB.X, frustumCorner.X);
                MinFrustumAABB.Y = MathUtil.Min(MinFrustumAABB.Y, frustumCorner.Y);
                MinFrustumAABB.Z = MathUtil.Min(MinFrustumAABB.Z, frustumCorner.Z);

                MaxFrustumAABB.X = MathUtil.Max(MaxFrustumAABB.X, frustumCorner.X);
                MaxFrustumAABB.Y = MathUtil.Max(MaxFrustumAABB.Y, frustumCorner.Y);
                MaxFrustumAABB.Z = MathUtil.Max(MaxFrustumAABB.Z, frustumCorner.Z);
            }
        }

        /// <summary>
        /// カリングされるか
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="margin"></param>
        /// <returns>視錐台内にあればtrue。残って描画される方がtrue</returns>
        public bool IsCulled(AffineTransform transform, float margin)
        {
            if (transform.Translation.X > MaxFrustumAABB.X + margin)
                return false;

            if (transform.Translation.Y > MaxFrustumAABB.Y + margin)
                return false;

            if (transform.Translation.Z > MaxFrustumAABB.Z + margin)
                return false;

            if (transform.Translation.X < MinFrustumAABB.X - margin)
                return false;

            if (transform.Translation.Y < MinFrustumAABB.Y - margin)
                return false;

            if (transform.Translation.Z < MinFrustumAABB.Z - margin)
                return false;

            return true;
        }
    }
}
