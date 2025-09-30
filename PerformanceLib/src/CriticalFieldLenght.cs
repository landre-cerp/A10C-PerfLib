namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    /// <summary>
    /// Calculate the Critical Field Length in Feet, speedbrakes opened 100%
    /// </summary>
    public static double CriticalFieldLength(TakeoffIndex takeoffIndex, GrossWeight grossWeight, double windspeed, FLAPS flaps, RCR rcr)
    {
        if (flaps != FLAPS.TO)
            throw new ArgumentException("Flaps configuration not yet handled");

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
            // increase by 7%
        }
        // same with speedbrakes not opened + 7% 

        throw new NotImplementedException("Critical Field Length calculation not yet implemented");
    }

    // Overload pour compatibilité
    public static double CriticalFieldLength(double takeoffIndex, double grossWeight, double windspeed, FLAPS flaps, RCR rcr)
    {
        return CriticalFieldLength(new TakeoffIndex(takeoffIndex), new GrossWeight(grossWeight), windspeed, flaps, rcr);
    }
}
