using a10c_perf_lib.src.Tables.Takeoff;
using a10c_perf_lib.src.Tables.Climb;
using a10c_perf_lib.src.Tables.Descent;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    // Singleton instances of correction tables
    internal static readonly WindCorrectionTable GroundWindCorrection = new();
    internal static readonly RcrCorrectionTable RcrDistanceCorrection = new();

    // Climb temperature correction tables
    internal static readonly ClimbDistanceTempTable ClimbDistTemp = new();
    internal static readonly ClimbFuelTempTable     ClimbFuelTemp = new();
    internal static readonly ClimbTimeTempTable     ClimbTimeTemp = new();

    // Descent max-range speed table
    internal static readonly MaxRangeSpeedTable MaxRangeDescentSpeedInstance = new();
}
