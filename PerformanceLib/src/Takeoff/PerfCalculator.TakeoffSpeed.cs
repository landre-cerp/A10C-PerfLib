namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static double TakeOffSpeed(double grossWeight, FLAPS flaps)
    {
        if (flaps == FLAPS.UP)
            return 52.4 + grossWeight * (2.67e-3 + grossWeight * -1.1e-8);

        return 43.8 + grossWeight * (3.11e-3 + grossWeight * (-2.38e-8 + grossWeight * 1.08e-13));
    }
}
