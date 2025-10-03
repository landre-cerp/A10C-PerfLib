using a10c_perf_lib.src.CorrectionTables;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    // Singleton instances for fifty-foot obstacle clearance tables
    private static readonly FiftyFootObstacleClearanceFlapsToTable _flapsToTable = new();
    private static readonly FiftyFootObstacleClearanceFlapsUpTable _flapsUpTable = new();

    /// <summary>
    /// Calculate the fifty-foot obstacle clearance distance in feet
    /// </summary>
    public static double FiftyFootObstacleClearanceDistance(double groundRun, double windspeed, FLAPS flaps)
    {
        CorrectionTable tableToUse = flaps == FLAPS.UP ? _flapsUpTable : _flapsToTable;
        return tableToUse.Interpolate(groundRun, windspeed);
    }

    public static double FiftyFootObstacleClearanceDistance(double takeoffIndex, double grossWeight, double windspeed, FLAPS flaps)
    {
        return FiftyFootObstacleClearanceDistance(new TakeoffIndex(takeoffIndex), new GrossWeight(grossWeight), windspeed, flaps);
    }
}
