namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    public static TakeoffResult Takeoff(
        TakeoffIndex index,
        GrossWeight grossWeight,
        double windKts,
        FLAPS flaps,
        ThrustSetting thrust,
        RCR rcr)
    {
        double groundRun = TakeoffGroundRun(index, grossWeight, windKts, flaps);
        double speed     = TakeOffSpeed(grossWeight, flaps);

        return new TakeoffResult(
            GroundRunFt:        Math.Ceiling(groundRun),
            FiftyFtClearanceFt: Math.Ceiling(FiftyFtObstacleClearanceDistance(groundRun, windKts, flaps, thrust)),
            CriticalFieldFt:    Math.Ceiling(CriticalFieldLength(index, grossWeight, windKts, flaps, rcr)),
            TakeoffSpeedKts:    speed,
            RotationSpeedKts:   speed - 10
        );
    }

    public static TakeoffResult Takeoff(double index, double grossWeight, double windKts, FLAPS flaps, ThrustSetting thrust, RCR rcr)
        => Takeoff(new TakeoffIndex(index), new GrossWeight(grossWeight), windKts, flaps, thrust, rcr);
}
