using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HimaLib.Math
{
    public class SystemRand : IRand
    {
        public int Seed { get; set; }

        Random rand;

        public SystemRand()
            : this(Environment.TickCount)
        {
        }

        public SystemRand(int seed)
        {
            Init(seed);
        }

        public void Init(int s)
        {
            Seed = s;
            rand = new Random(Seed);
        }

        public int Next()
        {
            return rand.Next();
        }

        public int Next(int max)
        {
            return rand.Next(max);
        }

        public int Next(int min, int max)
        {
            return rand.Next(min, max);
        }

        public float NextFloat()
        {
            return (float)rand.NextDouble();
        }

        public float NextFloat(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}
