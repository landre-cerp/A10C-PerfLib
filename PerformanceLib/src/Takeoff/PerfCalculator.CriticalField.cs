using a10c_perf_lib.src.Tables.Takeoff;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static readonly CriticalFieldLengthTable CriticalFieldLengthTable = new();

    private static double CriticalFieldLength(TakeoffIndex takeoffIndex, GrossWeight grossWeight, double windspeed, FLAPS flaps, RCR rcr)
    {
        double cfl = CriticalFieldLengthTable.Interpolate(takeoffIndex, grossWeight);
        if (cfl < 0)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), "Aircraft too heavy for the specified takeoff index");
        cfl = GroundWindCorrection.Interpolate(cfl, windspeed);
        cfl = RcrDistanceCorrection.Interpolate(cfl, (double)rcr);

        if (flaps == FLAPS.UP)
            cfl *= 1.07;

        return cfl;
    }
}
