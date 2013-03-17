using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Math;
using HimaLib.Camera;
using ccm.Particle;

namespace ccm.Deco
{
    public class Deco_Twister : Deco
    {
        public Deco_Twister(AffineTransform transform, ICamera camera, HimaLib.Math.IRand rand)
        {
            for (var i = 0; i < 16; ++i)
            {
                Particles.Add(new ccm.Particle.Particle_Twister(
                    transform,
                    camera,
                    rand.NextFloat() * 100.0f + 900.0f,
                    rand.NextFloat() * 360.0f,
                    rand.NextFloat() * 180.0f + 300.0f,
                    rand.NextFloat() * 10.0f + 5.0f));
            }
        }

        public override bool Update()
        {
            UpdateParticles();

            return Particles.Count > 0;
        }
    }
}
