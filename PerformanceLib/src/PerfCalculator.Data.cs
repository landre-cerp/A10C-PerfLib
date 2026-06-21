using a10c_perf_lib.src.CorrectionTables;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    // Singleton instances of correction tables
    internal static readonly GroundWindCorrectionTable GroundWindCorrection = new();
    internal static readonly RcrDistanceCorrectionTable RcrDistanceCorrection = new();

    // Landing correction tables
    internal static readonly LandingWindCorrectionTable LandingWindCorrection = new();
    internal static readonly LandingRcrCorrectionTable LandingRcrCorrection = new();
}