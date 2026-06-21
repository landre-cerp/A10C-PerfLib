namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    public static double ApproachSpeed(GrossWeight grossWeight, LandingFlaps flaps, bool singleEngine = false, bool minSpeed = false)
    {
        double w = grossWeight;

        if (singleEngine)
            return 120 + 0.001 * w;

        if (minSpeed)
            return 50 + 0.002 * w;

        return flaps == LandingFlaps.SEVEN
            ? 70 + 0.002 * w
            : 60 + 0.002 * w;
    }

    public static double TouchdownSpeed(GrossWeight grossWeight, LandingFlaps flaps, bool singleEngine = false, bool minSpeed = false)
    {
        double approach = ApproachSpeed(grossWeight, flaps, singleEngine, minSpeed);
        return minSpeed ? approach : approach - 10;
    }
}
