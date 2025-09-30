namespace a10c_perf_lib.src;

/// <summary>
/// Represents a validated Aircraft Gross Weight in pounds
/// </summary>
public readonly record struct GrossWeight
{
    private readonly double _value;

    public double Value => _value;
    public double Pounds => _value;

    public GrossWeight(double pounds)
    {
        if (pounds < PerfCalculator.EMPTY_WEIGHT_LBS || pounds > PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS)
            throw new ArgumentOutOfRangeException(nameof(pounds), pounds, 
                $"Grossweight out of range ({PerfCalculator.EMPTY_WEIGHT_LBS} to {PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS} lbs)");
        _value = pounds;
    }

    public static implicit operator double(GrossWeight weight) => weight._value;
    public static explicit operator GrossWeight(double pounds) => new(pounds);

    public override string ToString() => $"{_value:F0} lbs";
}