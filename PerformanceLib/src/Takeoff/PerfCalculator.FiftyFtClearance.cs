using a10c_perf_lib.src.Tables.Takeoff.FiftyFt;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static double FiftyFtObstacleClearanceDistance(double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrust)
    {
        return FiftyFtRegistry.Get(flaps, thrust).Interpolate(groundRun, windspeed);
    }
}
