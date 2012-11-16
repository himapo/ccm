using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ccm.CameraOld;

namespace ccm
{
    class FrustumCulling
    {
        static FrustumCulling instance = new FrustumCulling();

        Dictionary<CameraLabel, ViewFrustum> frustumDic;

        class ViewFrustum
        {
            public Plane near;
            public Plane far;
            public Plane top;
            public Plane bottom;
            public Plane left;
            public Plane right;
        }

        public static FrustumCulling GetInstance()
        {
            return instance;
        }

        FrustumCulling()
        {
            frustumDic = new Dictionary<CameraLabel, ViewFrustum>();
        }

        public void ClearFrustum()
        {
            frustumDic.Clear();
        }

        public bool IsCulled(CameraLabel cameraLabel, Model model, Matrix world)
        {
            var frustum = GetViewFrustum(cameraLabel);

            // メッシュが１つでもカリング失敗したら無効
            foreach (var mesh in model.Meshes)
            {
                var sphere = mesh.BoundingSphere;
                sphere.Center = TransformCoord(sphere.Center, world);
                if (!DetectFrustumCollision(frustum, sphere))
                {
                    return false;
                }
            }

            return true;
        }

        ViewFrustum GetViewFrustum(CameraLabel cameraLabel)
        {
            ViewFrustum frustum;
            if (!frustumDic.TryGetValue(cameraLabel, out frustum))
            {
                var camera = CameraManager.GetInstance().Get(cameraLabel);
                frustum = CalcViewFrustum(camera.View, camera.Proj);
                frustumDic[cameraLabel] = frustum;
            }
            return frustum;
        }

        ViewFrustum CalcViewFrustum(Matrix view, Matrix proj)
        {
            var matVP = Matrix.Multiply(view, proj);
            var invVP = Matrix.Invert(matVP);

            var near_tl = new Vector3(-1, 1, 0);
            var near_tr = new Vector3(1, 1, 0);
            var near_bl = new Vector3(-1, -1, 0);
            var near_br = new Vector3(1, -1, 0);
            var far_tl = new Vector3(-1, 1, 1);
            var far_tr = new Vector3(1, 1, 1);
            var far_bl = new Vector3(-1, -1, 1);
            var far_br = new Vector3(1, -1, 1);

            near_tl = TransformCoord(near_tl, invVP);
            near_tr = TransformCoord(near_tr, invVP);
            near_bl = TransformCoord(near_bl, invVP);
            near_br = TransformCoord(near_br, invVP);
            far_tl = TransformCoord(far_tl, invVP);
            far_tr = TransformCoord(far_tr, invVP);
            far_bl = TransformCoord(far_bl, invVP);
            far_br = TransformCoord(far_br, invVP);

            var plane_near = new Plane(near_bl, near_tl, near_tr);
            var plane_far = new Plane(far_tr, far_tl, far_bl);
            var plane_top = new Plane(near_tr, near_tl, far_tl);
            var plane_bottom = new Plane(far_bl, near_bl, near_br);
            var plane_left = new Plane(far_bl, far_tl, near_tl);
            var plane_right = new Plane(near_tr, far_tr, far_br);

            plane_near.Normalize();
            plane_far.Normalize();
            plane_top.Normalize();
            plane_bottom.Normalize();
            plane_left.Normalize();
            plane_right.Normalize();

            var frustum = new ViewFrustum
            {
                near = plane_near,
                far = plane_far,
                top = plane_top,
                bottom = plane_bottom,
                left = plane_left,
                right = plane_right
            };

            return frustum;
        }

        Vector3 TransformCoord(Vector3 position, Matrix matrix)
        {
            var vec4 = Vector4.Transform(position, matrix);
            vec4 /= vec4.W;
            return new Vector3(vec4.X, vec4.Y, vec4.Z);
        }

        bool DetectFrustumCollision(ViewFrustum frustum, BoundingSphere sphere)
        {
            if (frustum.near.Intersects(sphere) == PlaneIntersectionType.Back)
                return true;
            if (frustum.far.Intersects(sphere) == PlaneIntersectionType.Back)
                return true;
            if (frustum.top.Intersects(sphere) == PlaneIntersectionType.Back)
                return true;
            if (frustum.bottom.Intersects(sphere) == PlaneIntersectionType.Back)
                return true;
            if (frustum.left.Intersects(sphere) == PlaneIntersectionType.Back)
                return true;
            if (frustum.right.Intersects(sphere) == PlaneIntersectionType.Back)
                return true;

            return false;
        }
    }
}
