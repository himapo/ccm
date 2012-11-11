
namespace ccm
{
    interface IRand
    {
        int Seed { get; set; }

        void Init(int seed);

        int Next();

        int Next(int max);

        int Next(int min, int max);

        float NextFloat();

        float NextFloat(float min, float max);
    }
}
