using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.src.CorrectionTables;

internal static class FiftyFtClearanceTableRegistry
{
    // Cached singletons; static initialization is thread-safe.
    private static readonly CorrectionTable[,] Tables =
    {
        // Thrust: Max, ThreePercentBelow
        { new FiftyFtClearanceFlapsUpMaxThrustTable(), new FiftyFtClearanceFlapsUp3PercentBelowTable() }, // Flaps Up
        { new FiftyFtClearanceFlapsToMaxThrustTable(), new FiftyFtClearanceFlapsTo3PercentBelowTable() }  // Flaps TO (7°)
    };

    public static CorrectionTable Get(FLAPS flaps, ThrustSetting thrust)
        => Tables[(int)flaps, (int)thrust];
}