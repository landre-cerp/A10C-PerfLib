using a10c_perf_lib.src.Tables.Descent;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private const double DescentMaxAltFt  = 35000;
    private const double DescentMinWeight = 25000;
    private const double DescentMaxWeight = 50000;

    public static double DescentDistance(double startAltFt, double endAltFt, double grossWeight, double drag)
    {
        ValidateDescentInputs(startAltFt, endAltFt, grossWeight, drag);
        double start = DescentDistanceTable.Instance.Interpolate(startAltFt, grossWeight, drag);
        double end_  = DescentDistanceTable.Instance.Interpolate(endAltFt,   grossWeight, drag);
        return Math.Ceiling(start - end_);
    }

    public static double DescentFuel(double startAltFt, double endAltFt, double grossWeight, double drag)
    {
        ValidateDescentInputs(startAltFt, endAltFt, grossWeight, drag);
        double start = DescentFuelTable.Instance.Interpolate(startAltFt, grossWeight, drag);
        double end_  = DescentFuelTable.Instance.Interpolate(endAltFt,   grossWeight, drag);
        return Math.Ceiling(start - end_);
    }

    public static double DescentTime(double startAltFt, double endAltFt, double grossWeight, double drag)
    {
        ValidateDescentInputs(startAltFt, endAltFt, grossWeight, drag);
        double start = DescentTimeTable.Instance.Interpolate(startAltFt, grossWeight, drag);
        double end_  = DescentTimeTable.Instance.Interpolate(endAltFt,   grossWeight, drag);
        return Math.Ceiling(start - end_);
    }

    public static double MaxRangeDescentSpeed(double grossWeight, double drag)
    {
        if (grossWeight is < DescentMinWeight or > DescentMaxWeight)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), $"Gross weight must be between {DescentMinWeight} and {DescentMaxWeight} lbs");
        if (drag is < 0 or > 8)
            throw new ArgumentOutOfRangeException(nameof(drag), "Drag index must be between 0 and 8");
        return MaxRangeDescentSpeedInstance.Interpolate(grossWeight, drag);
    }

    private static void ValidateDescentInputs(double startAltFt, double endAltFt, double grossWeight, double drag)
    {
        if (startAltFt > DescentMaxAltFt)
            throw new ArgumentOutOfRangeException(nameof(startAltFt), $"Start altitude must be ≤ {DescentMaxAltFt} ft");
        if (endAltFt < 0 || endAltFt > startAltFt)
            throw new ArgumentException("End altitude must be ≥ 0 and ≤ start altitude for a descent");
        if (grossWeight is < DescentMinWeight or > DescentMaxWeight)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), $"Gross weight must be between {DescentMinWeight} and {DescentMaxWeight} lbs");
        if (drag is < 0 or > 8)
            throw new ArgumentOutOfRangeException(nameof(drag), "Drag index must be between 0 and 8");
    }
}
