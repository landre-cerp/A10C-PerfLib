using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.src.Tables.Takeoff.FiftyFt;

internal static class FiftyFtRegistry
{
    // Cached singletons; static initialization is thread-safe.
    private static readonly CorrectionTable[,] Tables =
    {
        // Thrust: Max, ThreePercentBelow
        { new FlapsUpMaxThrustTable(), new FlapsUp3PctBelowTable() }, // Flaps Up
        { new FlapsToMaxThrustTable(), new FlapsTo3PctBelowTable() }  // Flaps TO (7°)
    };

    public static CorrectionTable Get(FLAPS flaps, ThrustSetting thrust)
        => Tables[(int)flaps, (int)thrust];
}
