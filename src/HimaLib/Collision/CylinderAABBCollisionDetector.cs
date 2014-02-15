using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;

namespace HimaLib.Collision
{
    public class CylinderAABBCollisionDetector : ICollisionDetector
    {
        public CylinderCollisionPrimitive Cylinder { get; set; }

        public AABBCollisionPrimitive AABB { get; set; }

        public bool Detect(CollisionResult result)
        {
            // 垂直交差判定
            var al = Cylinder.Base().Y;
            var ah = al + Cylinder.Height();
            var bl = AABB.Corner.Y;
            var bh = bl + AABB.Width.Y;
            var vertical =
                (al > bl && al < bh)
                || (ah > bl && ah < bh)
                || (bl > al && bl < ah)
                || (bh > al && bh < ah);

            if (!vertical)
            {
                result.Overlap = Vector3.Zero;
                return false;
            }

            {
                // Aの下とBの上がめり込んでると見る場合
                var a_bottom_b_top = al - bh;

                // Aの上とBの下がめり込んでると見る場合
                var a_top_b_bottom = ah - bl;

                // めり込み量が少ない方を採用
                result.Overlap.Y =
                    MathUtil.Abs(a_bottom_b_top) < MathUtil.Abs(a_top_b_bottom)
                    ? a_bottom_b_top
                    : a_top_b_bottom;
            }

            // 水平交差判定

            if (Cylinder.Base().X + Cylinder.Radius() < AABB.Corner.X
                || Cylinder.Base().X - Cylinder.Radius() > AABB.Corner.X + AABB.Width.X)
            {
                result.Overlap = Vector3.Zero;
                return false;
            }

            if (Cylinder.Base().Z + Cylinder.Radius() < AABB.Corner.Z
                || Cylinder.Base().Z - Cylinder.Radius() > AABB.Corner.Z + AABB.Width.Z)
            {
                result.Overlap = Vector3.Zero;
                return false;
            }

            var horizontal = false;

            if (Cylinder.Base().X > AABB.Corner.X
                && Cylinder.Base().X < AABB.Corner.X + AABB.Width.X)
            {
                horizontal = true;
            }

            {
                // 円が箱の左辺から右方向にめり込んでると見る場合
                var a_right_b_left = (Cylinder.Base().X + Cylinder.Radius()) - AABB.Corner.X;

                // 円が箱の右辺から左方向にめり込んでると見る場合
                var a_left_b_right = (Cylinder.Base().X - Cylinder.Radius()) - (AABB.Corner.X + AABB.Width.X);

                // めり込み量が少ない方を採用
                result.Overlap.X =
                    MathUtil.Abs(a_right_b_left) < MathUtil.Abs(a_left_b_right)
                    ? a_right_b_left
                    : a_left_b_right;
            }

            if (Cylinder.Base().Z > AABB.Corner.Z
                && Cylinder.Base().Z < AABB.Corner.Z + AABB.Width.Z)
            {
                horizontal = true;
            }

            {
                // 円が箱の上辺から下方向にめり込んでると見る場合
                var a_bottom_b_top = (Cylinder.Base().Z + Cylinder.Radius()) - AABB.Corner.Z;

                // 円が箱の下辺から上方向にめり込んでると見る場合
                var a_top_b_bottom = (Cylinder.Base().Z - Cylinder.Radius()) - (AABB.Corner.Z + AABB.Width.Z);

                // めり込み量が少ない方を採用
                result.Overlap.Z =
                    MathUtil.Abs(a_bottom_b_top) < MathUtil.Abs(a_top_b_bottom)
                    ? a_bottom_b_top
                    : a_top_b_bottom;
            }

            if (horizontal)
            {
                return true;
            }

            // 4つの角のうち最短距離が筒の半径以内か

            var corners = new List<Vector2>();

            corners.Add(new Vector2(AABB.Corner.X, AABB.Corner.Z));
            corners.Add(new Vector2(AABB.Corner.X + AABB.Width.X, AABB.Corner.Z));
            corners.Add(new Vector2(AABB.Corner.X + AABB.Width.X, AABB.Corner.Z + AABB.Width.Z));
            corners.Add(new Vector2(AABB.Corner.X, AABB.Corner.Z + AABB.Width.Z));

            var cylinderCenter = new Vector2(Cylinder.Base().X, Cylinder.Base().Z);

            var minLength = corners.Min((corner) =>
            {
                return (corner - cylinderCenter).LengthSquared();
            });

            if (minLength >= (Cylinder.Radius() * Cylinder.Radius()))
            {
                result.Overlap = Vector3.Zero;
                return false;
            }

            // 最短距離の角
            var minCorner = corners.First((corner) =>
            {
                return minLength == (corner - cylinderCenter).LengthSquared();
            });

            {
                // 円が箱の左辺から右方向にめり込んでると見る場合
                var a_right_b_left = (cylinderCenter.X + Cylinder.Radius()) - minCorner.X;

                // 円が箱の右辺から左方向にめり込んでると見る場合
                var a_left_b_right = (cylinderCenter.X - Cylinder.Radius()) - minCorner.X;

                // めり込み量が少ない方を採用
                result.Overlap.X =
                    MathUtil.Abs(a_right_b_left) < MathUtil.Abs(a_left_b_right)
                    ? a_right_b_left
                    : a_left_b_right;
            }

            {
                // 円が箱の上辺から下方向にめり込んでると見る場合
                var a_bottom_b_top = (cylinderCenter.Y + Cylinder.Radius()) - minCorner.Y;

                // 円が箱の下辺から上方向にめり込んでると見る場合
                var a_top_b_bottom = (cylinderCenter.Y - Cylinder.Radius()) - minCorner.Y;

                // めり込み量が少ない方を採用
                result.Overlap.Z =
                    MathUtil.Abs(a_bottom_b_top) < MathUtil.Abs(a_top_b_bottom)
                    ? a_bottom_b_top
                    : a_top_b_bottom;
            }

            return true;
        }
    }
}
