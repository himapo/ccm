using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Updater;
using HimaLib.Camera;
using HimaLib.Texture;

namespace ccm.Particle
{
    public class Particle_Twister : Particle
    {
        SimpleBillboardRenderParameter SimpleBillboardRenderParameter = new SimpleBillboardRenderParameter();

        Vector3 InitialPosition;

        AffineTransform Transform;

        bool Alive = true;

        protected override BillboardRenderParameter BillboardRenderParameter
        {
            get { return SimpleBillboardRenderParameter; }
        }

        public Particle_Twister(
            HimaLib.Math.AffineTransform transform, 
            CameraBase camera,
            float finishMilliSeconds,
            float startDegree,
            float degreeDelta,
            float endHeightDelta,
            string textureName,
            bool loop)
        {
            InitialPosition = new Vector3(transform.Translation);
            Transform = new AffineTransform(transform);

            SimpleBillboardRenderParameter.Camera = camera;
            SimpleBillboardRenderParameter.Alpha = 0.8f;

            SimpleBillboardRenderParameter.AffineTransform = Transform;
            SimpleBillboardRenderParameter.AffineTransform.Scale = Vector3.One * 0.04f;
            SimpleBillboardRenderParameter.AffineTransform.Rotation = Vector3.Zero;

            SimpleBillboardRenderParameter.Texture = TextureFactory.Instance.CreateFromImage(textureName);

            if (loop)
            {
                CreateUpdater(finishMilliSeconds, startDegree, degreeDelta, endHeightDelta);
            }
            else
            {
                CreateUpdater(finishMilliSeconds, startDegree, degreeDelta, endHeightDelta, () => { Alive = false; });
            }
        }

        void CreateUpdater(
           float finishMilliSeconds,
           float startDegree,
           float degreeDelta,
           float endHeightDelta,
           Action callback)
        {
            new HimaLib.Updater.CylinderUpdater(
                UpdaterManager,
                finishMilliSeconds,
                startDegree,
                startDegree + degreeDelta,
                new Vector2(InitialPosition.X, InitialPosition.Z),
                new Vector2(3.0f, 3.0f),
                a => { Transform.Translation.X = a; },
                a => { Transform.Translation.Z = a; },
                InitialPosition.Y,
                InitialPosition.Y + endHeightDelta,
                a => { Transform.Translation.Y = a; },
                callback);
        }

        void CreateUpdater(
            float finishMilliSeconds,
            float startDegree,
            float degreeDelta,
            float endHeightDelta)
        {
            CreateUpdater(
                finishMilliSeconds,
                startDegree,
                degreeDelta,
                endHeightDelta,
                () =>
                {
                    CreateUpdater(
                        finishMilliSeconds,
                        startDegree,
                        degreeDelta,
                        endHeightDelta
                        );
                });
        }

        public override bool Update()
        {
            return Alive;
        }
    }
}
