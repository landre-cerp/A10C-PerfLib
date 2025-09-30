namespace a10c_perf_lib.src;

/// <summary>
/// Represents a validated Takeoff Index value between 4.0 and 11.0
/// </summary>
public readonly record struct TakeoffIndex
{
    private readonly double _value;

    public double Value => _value;

    public TakeoffIndex(double value)
    {
        if (value < 4.0 || value > 11.0)
            throw new ArgumentOutOfRangeException(nameof(value), value, "Takeoff Index out of range (4.0 to 11.0)");
        _value = value;
    }

    public static implicit operator double(TakeoffIndex index) => index._value;
    public static explicit operator TakeoffIndex(double value) => new(value);

    public override string ToString() => _value.ToString("F1");
}