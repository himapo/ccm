using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Updater;
using HimaLib.Camera;

namespace ccm.Particle
{
    public class Particle_Twister : Particle
    {
        SimpleBillboardRenderParameter SimpleBillboardRenderParameter = new SimpleBillboardRenderParameter();

        AffineTransform Transform;

        bool Alive = true;

        protected override IBillboardRenderParameter BillboardRenderParameter
        {
            get { return SimpleBillboardRenderParameter; }
        }

        public Particle_Twister(
            HimaLib.Math.AffineTransform transform, 
            ICamera camera,
            float finishMilliSeconds,
            float startDegree,
            float degreeDelta,
            float endHeightDelta)
        {
            Transform = new AffineTransform(transform);

            new HimaLib.Updater.CylinderUpdater(
                UpdaterManager,
                finishMilliSeconds,
                startDegree,
                startDegree + degreeDelta,
                new Vector2(Transform.Translation.X, Transform.Translation.Z),
                new Vector2(5.0f, 5.0f),
                a => { Transform.Translation.X = a; },
                a => { Transform.Translation.Z = a; },
                Transform.Translation.Y,
                Transform.Translation.Y + endHeightDelta,
                a => { Transform.Translation.Y = a; },
                () => { Alive = false; });

            SimpleBillboardRenderParameter.Camera = camera;
            SimpleBillboardRenderParameter.Alpha = 0.8f;
            SimpleBillboardRenderParameter.Transform = Transform;
            SimpleBillboardRenderParameter.Transform.Scale = Vector3.One * 0.04f;
            SimpleBillboardRenderParameter.Transform.Rotation = Vector3.Zero;
            SimpleBillboardRenderParameter.Transform.Translation = Transform.Translation;
            SimpleBillboardRenderParameter.TextureName = "Texture/particle000";
        }

        public override bool Update()
        {
            return Alive;
        }
    }
}
