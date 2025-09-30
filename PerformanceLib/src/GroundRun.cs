namespace a10c_perf_lib.src;

public partial class PerfCalculator
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
    public double TakeoffGroundRun(TakeoffIndex takeoffIndex, GrossWeight grossWeight, double windSpeed)
    {
        double groundRun = PerfCalculatorHelpers.BilinearInterpolate(
            TakeoffGroundRunTable,
            takeoffindexes,
            Grossweights,
            takeoffIndex,
            grossWeight);
        
        if (groundRun == -1)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), $"Aircraft too heavy for the specified takeoff index (takeoffIndex={takeoffIndex}, grossWeight={grossWeight})");

        if (groundRun < 1000)
            return groundRun; // no correction for short distances

        groundRun = PerfCalculatorHelpers.BilinearInterpolate(
            GroundWindCorrection,
            distances,
            GroundRunWinds,
            groundRun,
            windSpeed);

        return groundRun;
    }

    // Overload pour compatibilité
    public double TakeoffGroundRun(double takeoffIndex, double grossWeight, double windSpeed)
    {
        return TakeoffGroundRun(new TakeoffIndex(takeoffIndex), new GrossWeight(grossWeight), windSpeed);
    }
}
