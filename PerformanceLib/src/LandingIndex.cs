namespace a10c_perf_lib.src;

/// <summary>
/// Represents a validated Landing Index value between 70.0 and 120.0
/// </summary>
public readonly record struct LandingIndex
{
    private readonly double _value;

    public double Value => _value;

    public LandingIndex(double value)
    {
        if (value < 70.0 || value > 120.0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Landing Index out of range (70.0 to 120.0)");
        _value = value;
    }

    public static implicit operator double(LandingIndex index) => index._value;
    public static explicit operator LandingIndex(double value) => new(value);

    public override string ToString() => _value.ToString("F1");
}
