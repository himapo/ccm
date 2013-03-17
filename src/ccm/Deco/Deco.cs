using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ccm.Particle;

namespace ccm.Deco
{
    public abstract class Deco
    {
        List<ccm.Particle.Particle> AliveList = new List<Particle.Particle>();

        List<ccm.Particle.Particle> DeleteList = new List<Particle.Particle>();

        protected List<ccm.Particle.Particle> Particles
        {
            get { return AliveList; }
        }

        public abstract bool Update();

        protected void UpdateParticles()
        {
            AliveList.ForEach((a) =>
            {
                if (!a.Update())
                {
                    DeleteList.Add(a);
                }
            });

            DeleteList.ForEach((a) => { AliveList.Remove(a); });
            DeleteList.Clear();
        }

        public virtual void Draw()
        {
            DrawParticles();
        }

        protected void DrawParticles()
        {
            AliveList.ForEach((a) => { a.Draw(); });
        }
    }
}
