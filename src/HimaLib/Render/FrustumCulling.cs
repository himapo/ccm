using System;
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
        /// 視錐台の面
        /// </summary>
        Plane[] Planes = new Plane[6];

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
                new Vector3(-1, 1, 0),  // 0 near_tl
                new Vector3(1, 1, 0),   // 1 near_tr
                new Vector3(-1, -1, 0), // 2 near_bl
                new Vector3(1, -1, 0),  // 3 near_br
                new Vector3(-1, 1, 1),  // 4 far_tl
                new Vector3(1, 1, 1),   // 5 far_tr
                new Vector3(-1, -1, 1), // 6 far_bl
                new Vector3(1, -1, 1),  // 7 far_br
            };

            var worldCorners = projCorners.Select(c =>
            {
                return Vector3.TransformCoord(c, invViewProj);
            }).ToArray();

            // 法線が視錐台の外側を向くように面を作る（右手系）
            Planes[0] = new Plane(worldCorners[0], worldCorners[2], worldCorners[1]);   // Near
            Planes[1] = new Plane(worldCorners[5], worldCorners[7], worldCorners[4]);   // Far
            Planes[2] = new Plane(worldCorners[0], worldCorners[1], worldCorners[4]);   // Top
            Planes[3] = new Plane(worldCorners[2], worldCorners[6], worldCorners[3]);   // Bottom
            Planes[4] = new Plane(worldCorners[0], worldCorners[4], worldCorners[2]);   // Left
            Planes[5] = new Plane(worldCorners[1], worldCorners[3], worldCorners[5]);   // Right

            // 簡易版は視錐台に外接するAABBでカリングする
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
            return IsCulled(
                transform.Translation,
                margin);
        }

        public bool IsCulled(Matrix transform, float margin)
        {
            return IsCulled(
                new Vector3(transform.M41, transform.M42, transform.M43),
                margin);
        }

        bool IsCulled(Vector3 center, float margin)
        {
            foreach (var plane in Planes)
            {
                // 視錐台平面から点までの法線方向の距離が
                // バウンティング球の半径より離れていたら描画しない
                if (margin < plane.DotCoordinate(center))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 簡易版
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="margin"></param>
        /// <returns></returns>
        public bool IsCulledLight(AffineTransform transform, float margin)
        {
            return IsCulledLight(
                transform.Translation.X,
                transform.Translation.Y,
                transform.Translation.Z,
                margin);
        }

        public bool IsCulledLight(Matrix transform, float margin)
        {
            return IsCulledLight(
                transform.M41,
                transform.M42,
                transform.M43,
                margin);
        }

        bool IsCulledLight(float x, float y, float z, float margin)
        {
            if (x > MaxFrustumAABB.X + margin)
                return false;

            if (y > MaxFrustumAABB.Y + margin)
                return false;

            if (z > MaxFrustumAABB.Z + margin)
                return false;

            if (x < MinFrustumAABB.X - margin)
                return false;

            if (y < MinFrustumAABB.Y - margin)
                return false;

            if (z < MinFrustumAABB.Z - margin)
                return false;

            return true;
        }
    }
}
