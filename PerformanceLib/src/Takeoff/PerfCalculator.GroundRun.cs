using a10c_perf_lib.src.Tables.Takeoff;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static readonly GroundRunFlapsTOTable TakeoffGroundRunTableFlapsTO = new();
    private static readonly GroundRunFlapsUpTable TakeoffGroundRunTableFlapsUp = new();

    private static double TakeoffGroundRun(TakeoffIndex takeoffIndex, GrossWeight grossWeight, double windSpeed, FLAPS flaps)
    {
        CorrectionTable tableToUse = flaps == FLAPS.UP ? TakeoffGroundRunTableFlapsUp : TakeoffGroundRunTableFlapsTO;
        double groundRun = tableToUse.Interpolate(takeoffIndex, grossWeight);

        if (groundRun == -1)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), "Aircraft too heavy for the specified takeoff index");

        if (groundRun < 1000)
            return groundRun;

        return GroundWindCorrection.Interpolate(groundRun, windSpeed);
    }
}
