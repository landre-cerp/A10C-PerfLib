using a10c_perf_lib.src.Tables.Climb;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private const double ClimbMinAltFt  = 25000;
    private const double ClimbMaxAltFt  = 50000;
    private const double ClimbMinWeight = 25000;
    private const double ClimbMaxWeight = 50000;

    public static double ClimbDistance(double startAltFt, double targetAltFt, double grossWeight, double drag, double deltaISA = 0)
    {
        ValidateClimbInputs(startAltFt, targetAltFt, grossWeight, drag);
        double target = Math.Max(0, ClimbDistanceTable.Instance.Interpolate(grossWeight, targetAltFt, drag));
        double start  = Math.Max(0, ClimbDistanceTable.Instance.Interpolate(grossWeight, startAltFt,  drag));
        double delta  = target - start;
        if (delta <= 0) return 0;
        if (deltaISA != 0) delta = Math.Max(0, ClimbDistTemp.Interpolate(Math.Clamp(delta, 0, 125), Math.Clamp(deltaISA, -40, 60)));
        return Math.Ceiling(delta * 10) / 10;
    }

    public static double ClimbFuel(double startAltFt, double targetAltFt, double grossWeight, double drag, double deltaISA = 0)
    {
        ValidateClimbInputs(startAltFt, targetAltFt, grossWeight, drag);
        double target = Math.Max(0, ClimbFuelTable.Instance.Interpolate(grossWeight, targetAltFt, drag));
        double start  = Math.Max(0, ClimbFuelTable.Instance.Interpolate(grossWeight, startAltFt,  drag));
        double delta  = target - start;
        if (delta <= 0) return 0;
        if (deltaISA != 0) delta = Math.Max(0, ClimbFuelTemp.Interpolate(Math.Clamp(delta, 250, 2000), Math.Clamp(deltaISA, -40, 60)));
        return Math.Ceiling(delta / 10.0) * 10;
    }

    public static double ClimbTime(double startAltFt, double targetAltFt, double grossWeight, double drag, double deltaISA = 0)
    {
        ValidateClimbInputs(startAltFt, targetAltFt, grossWeight, drag);
        double target = Math.Max(0, ClimbTimeTable.Instance.Interpolate(grossWeight, targetAltFt, drag));
        double start  = Math.Max(0, ClimbTimeTable.Instance.Interpolate(grossWeight, startAltFt,  drag));
        double delta  = target - start;
        if (delta <= 0) return 0;
        if (deltaISA != 0) delta = Math.Max(0, ClimbTimeTemp.Interpolate(Math.Clamp(delta, 0, 35), Math.Clamp(deltaISA, -40, 60)));
        return Math.Ceiling(delta);
    }

    private static void ValidateClimbInputs(double startAltFt, double targetAltFt, double grossWeight, double drag)
    {
        if (targetAltFt > ClimbMaxAltFt)
            throw new ArgumentOutOfRangeException(nameof(targetAltFt), $"Target altitude must be ≤ {ClimbMaxAltFt} ft");
        if (startAltFt > targetAltFt)
            throw new ArgumentException("Start altitude must be ≤ target altitude for a climb");
        if (grossWeight is < ClimbMinWeight or > ClimbMaxWeight)
            throw new ArgumentOutOfRangeException(nameof(grossWeight), $"Gross weight must be between {ClimbMinWeight} and {ClimbMaxWeight} lbs");
        if (drag is < 0 or > 8)
            throw new ArgumentOutOfRangeException(nameof(drag), "Drag index must be between 0 and 8");
    }
}
