
namespace a10c_perf_lib.src;

public static class ValidationExtensions
{
    public static double ValidateTakeoffIndex(this double takeoffIndex)
    {
        if (takeoffIndex < 4.0 || takeoffIndex > 11.0)
            throw new ArgumentOutOfRangeException(nameof(takeoffIndex), takeoffIndex, "Takeoff Index out of range (4.0 to 11.0)");
        return takeoffIndex;
    }

    public static double ValidateGrossWeight(this double grossWeight)
    {
        if (grossWeight < PerfCalculator.EMPTY_WEIGHT_LBS || grossWeight > PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), grossWeight, 
                $"Grossweight out of range ({PerfCalculator.EMPTY_WEIGHT_LBS} to {PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS} lbs)");
        return grossWeight;
    }
}