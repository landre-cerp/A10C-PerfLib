using a10c_perf_lib.src.CorrectionTables;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static readonly LandingIndexTable LandingIndexTable = new();

    public static LandingIndex CalcLandingIndex(double tempC, PressureAltitude altitude)
    {
        return new LandingIndex(
            Math.Clamp(
                LandingIndexTable.Interpolate(tempC, altitude.Feet),
                70.0, 120.0
            )
        );
    }
}
