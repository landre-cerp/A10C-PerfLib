namespace a10c_perf_lib.src;

public partial class A10CPerfCalculator
{
    /// <summary>
    /// Calculate the Takeoff Ground Run in Feet
    /// </summary>
    /// <param name="takeoffIndex">calculated takeoff index</param>
    /// <param name="grossWeight">Aircraft GW in Lbs </param>
    /// <param name="windSpeed">Head or tailwind component, tail should be negative, head positive</param>
    /// <returns>The required distance to takeoff </returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="Exception"></exception>
    public double TakeoffGroundRun( double takeoffIndex, double grossWeight, double windSpeed)
    {
        if (takeoffIndex < 4.0 || takeoffIndex > 11.0)
            throw new ArgumentOutOfRangeException(nameof(takeoffIndex), "Takeoff Index out of range (4.0 to 11.0)");
        if (grossWeight < EMPTY_WEIGHT_LBS || grossWeight > MAX_TAKEOFF_WEIGHT_LBS)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), $"Grossweight out of range ({EMPTY_WEIGHT_LBS} to {MAX_TAKEOFF_WEIGHT_LBS} lbs)");

        double groundRun = PerfCalculatorHelpers.BilinearInterpolate(
            TakeoffGroundRunTable,
            takeoffindexes, Grossweights,
            takeoffIndex, grossWeight);
        if (groundRun == -1)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), $"Aircraft too heavy for the specified takeoff index (takeoffIndex={takeoffIndex}, grossWeight={grossWeight})");

        // Wind corrected distance with 10 knots 
        // should be lower that expected 
        if (groundRun < 1000)
            return groundRun; // no correction for short distances

        groundRun = PerfCalculatorHelpers.BilinearInterpolate(
            GroundRunWindCorrection,
            distances, GroundRunWinds,
            groundRun, windSpeed);

        return groundRun;
    }
}
