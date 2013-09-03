using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using ccm.Particle;

namespace ccm.Deco
{
    public class Deco_Shock : Deco
    {
        public Deco_Shock(AffineTransform transform, CameraBase camera)
        {
            for (var i = 0; i < 3; ++i)
            {
                Particles.Add(new ccm.Particle.Particle_Twister(
                    transform,
                    camera,
                    2000.0f,
                    120.0f * i,
                    360.0f,
                    0.0f,
                    "Texture/particle001",
                    true));
            }
        }

        public override bool Update()
        {
            UpdateParticles();

            return Particles.Count > 0;
        }
    }
}
