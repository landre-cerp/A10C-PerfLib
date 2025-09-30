namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    /// <summary>
    /// Calculate the Critical Field Length in Feet, speedbrakes opened 100%
    /// </summary>
    public static double CriticalFieldLength(TakeoffIndex takeoffIndex, GrossWeight grossWeight, double windspeed, FLAPS flaps, RCR rcr)
    {
        double criticalFieldLength = PerfCalculatorHelpers.BilinearInterpolate(
            CriticalFieldLengthTable,
            takeoffindexes, 
            Grossweights,
            takeoffIndex, 
            grossWeight);

        // Apply wind correction
        criticalFieldLength = PerfCalculatorHelpers.BilinearInterpolate(
            GroundWindCorrection,
            distances, 
            GroundRunWinds,
            criticalFieldLength, 
            windspeed);

        // Apply Runway Slope correction
        // Apply RCR correction

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
