namespace a10c_perf_lib.src;

public record TakeoffResult(
    double GroundRunFt,
    double FiftyFtClearanceFt,
    double CriticalFieldFt,
    double TakeoffSpeedKts,
    double RotationSpeedKts);
