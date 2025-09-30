namespace a10c_perf_lib.src;

public partial class PerfCalculator 
{
    public const double EMPTY_WEIGHT_LBS = 25629;
    public const double MAX_TAKEOFF_WEIGHT_LBS = 46476;

    public enum FLAPS
    {
        UP = 0,
        TO = 7,
    }

    public enum RCR
    {
        DRY = 23,
        WET = 12,
        ICY = 5,
    }

    public static double RCRWithoutAntiSkid(RCR rcr)
    {
        return 0.695 * (double)rcr;
    }
}
