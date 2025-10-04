using a10c_perf_lib.src.CorrectionTables;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    /// <summary>
    /// Calculates the fifty-feet obstacle clearance distance based on ground run, windspeed, flap setting.
    /// and whether Max thrust or 3% below PTFS thrust is used.
    /// </summary>
    /// <param name="groundRun">Calculated distance in feet</param>
    /// <param name="windspeed">Head wind component in knots</param>
    /// <param name="flaps">Flaps configuration for takeoff</param>
    /// <param name="threePercentBelow">use the 3 percent thrust. False by default</param>
    /// <returns>The distance needed to clear a 50 feet high obstacle</returns>
    public static double FiftyFtObstacleClearanceDistance(double groundRun, double windspeed, FLAPS flaps, ThrustSetting threePercentBelow)
    {
        return FiftyFtClearanceTableRegistry.Get(flaps, threePercentBelow).Interpolate(groundRun, windspeed);
    }
}
