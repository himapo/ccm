using System;

namespace ccm
{
    class SystemRand : IRand
    {
        public int Seed { get; set; }

        Random rand;

        void IRand.Init(int s)
        {
            Seed = s;
            rand = new Random(Seed);
        }

        int IRand.Next()
        {
            return rand.Next();
        }

        int IRand.Next(int max)
        {
            return rand.Next(max);
        }

        int IRand.Next(int min, int max)
        {
            return rand.Next(min, max);
        }

        float IRand.NextFloat()
        {
            return (float)rand.NextDouble();
        }

        float IRand.NextFloat(float min, float max)
        {
            return (float)rand.NextDouble() * (max - min) + min;
        }
    }
}
