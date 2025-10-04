using a10c_perf_lib.src.CorrectionTables;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static readonly CriticalFieldLengthTable CriticalFieldLengthTable = new();

    /// <summary>
    /// Calculate the Critical Field Length in Feet, speedbrakes opened 100%
    /// </summary>
    /// <param name="takeoffIndex">Calculated takeoff index</param>
    /// <param name="grossWeight">in lbs</param>
    /// <param name="windspeed">headwind in knots</param>
    /// <param name="flaps">Flaps config for takeoff</param>
    /// <param name="rcr">Runway condition</param>
    /// <returns>The critical distance to stop the plane in case of aborted takeoff</returns>
    public static double CriticalFieldLength(TakeoffIndex takeoffIndex, GrossWeight grossWeight, double windspeed, FLAPS flaps, RCR rcr)
    {
        // Base critical field length
        double criticalFieldLength = CriticalFieldLengthTable.Interpolate(takeoffIndex, grossWeight);

        // Apply wind correction
        criticalFieldLength = GroundWindCorrection.Interpolate(criticalFieldLength, windspeed);

        // Apply Runway Slope correction (not implemented yet)

        // Apply RCR correction
        criticalFieldLength = RcrDistanceCorrection.Interpolate(criticalFieldLength, (double)rcr);

        if (flaps == FLAPS.UP)
        {
            criticalFieldLength *= 1.07; 
        }
        // same with speedbrakes not opened + 7% 

        return criticalFieldLength;
    }

    public static double CriticalFieldLength(double takeoffIndex, double grossWeight, double windspeed, FLAPS flaps, RCR rcr)
    {
        return CriticalFieldLength(new TakeoffIndex(takeoffIndex), new GrossWeight(grossWeight), windspeed, flaps, rcr);
    }
}
