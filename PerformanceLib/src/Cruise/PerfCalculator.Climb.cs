using a10c_perf_lib.src.Tables.Climb;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private const double ClimbMinAltFt  = 25000;
    private const double ClimbMaxAltFt  = 50000;
    private const double ClimbMinWeight = 25000;
    private const double ClimbMaxWeight = 50000;

    public static ClimbResult Climb(double startAltFt, double targetAltFt, double grossWeight, double drag, double deltaISA = 0)
    {
        ValidateClimbInputs(startAltFt, targetAltFt, grossWeight, drag);

        double distance = ClimbDelta(ClimbDistanceTable.Instance.Interpolate, grossWeight, startAltFt, targetAltFt, drag);
        double fuel     = ClimbDelta(ClimbFuelTable.Instance.Interpolate,     grossWeight, startAltFt, targetAltFt, drag);
        double time     = ClimbDelta(ClimbTimeTable.Instance.Interpolate,     grossWeight, startAltFt, targetAltFt, drag);

        if (deltaISA != 0)
        {
            double isa = Math.Clamp(deltaISA, -40, 60);
            if (distance > 0) distance = Math.Max(0, ClimbDistTemp.Interpolate(Math.Clamp(distance, 0, 125),   isa));
            if (fuel     > 0) fuel     = Math.Max(0, ClimbFuelTemp.Interpolate(Math.Clamp(fuel,     250, 2000), isa));
            if (time     > 0) time     = Math.Max(0, ClimbTimeTemp.Interpolate(Math.Clamp(time,     0, 35),     isa));
        }

        return new ClimbResult(
            DistanceNm: distance > 0 ? Math.Ceiling(distance * 10) / 10 : 0,
            FuelLbs:    fuel     > 0 ? Math.Ceiling(fuel / 10.0) * 10   : 0,
            TimeMin:    time     > 0 ? Math.Ceiling(time)                : 0
        );
    }

    private static double ClimbDelta(Func<double, double, double, double> interpolate, double weight, double startAlt, double targetAlt, double drag)
    {
        double target = Math.Max(0, interpolate(weight, targetAlt, drag));
        double start  = Math.Max(0, interpolate(weight, startAlt,  drag));
        return Math.Max(0, target - start);
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
