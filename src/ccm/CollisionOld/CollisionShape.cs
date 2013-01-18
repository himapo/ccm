using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ccm
{
    /// <summary>
    /// コリジョン形状
    /// </summary>
    enum CollisionShape
    {
        Void,
        Rect,
        Triangle,
        Circle,
        Sphere,
        Ray,
        AABB,
        OBB,
        Cylinder,
    }

    abstract class ShapeParameter
    {
        public abstract CollisionShape Shape { get; }
    }

    // 球
    class SphereShapeParameter : ShapeParameter
    {
        public override CollisionShape Shape
        {
            get { return CollisionShape.Sphere; }
        }

        // 中心
        public Func<Vector3> Center { get; set; }

        // 半径
        public Func<float> Radius { get; set; }
    }

    // 円柱
    class CylinderShapeParameter : ShapeParameter
    {
        public override CollisionShape Shape
        {
            get { return CollisionShape.Cylinder; }
        }

        // 底面中心
        public Func<Vector3> Base { get; set; }

        // 半径
        public Func<float> Radius { get; set; }

        // 高さ
        public Func<float> Height { get; set; }
    }

}
