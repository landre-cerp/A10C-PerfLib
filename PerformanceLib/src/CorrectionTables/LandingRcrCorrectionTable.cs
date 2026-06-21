namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// RCR correction table for landing ground roll, flaps 20°.
/// X-axis: Distance (ft), Y-axis: RCR value (5=ICY, 12=WET, 23=DRY)
/// Data evaluated from TS cubic polynomial table in Landing.ts (RCR_landing_distance_flaps20).
/// NOTE: Verify against A10C-1 Chart A8-5 before use in production.
/// </summary>
internal sealed class LandingRcrCorrectionTable : CorrectionTable
{
    private static readonly double[] XAxis = { 500, 750, 1000, 1250, 1500, 1750, 2000, 2500, 3000, 3500 };
    private static readonly double[] YAxis = { 5, 12, 23 };

    private static readonly double[,] Matrix =
    {
        //  ICY=5   WET=12   DRY=23
        {    800,     659,     503 }, //  500 ft
        {   1750,    1274,     724 }, //  750 ft
        {   3200,    2088,     936 }, // 1000 ft
        {   4350,    2776,    1294 }, // 1250 ft
        {   5600,    3517,    1431 }, // 1500 ft
        {   6698,    4227,    1666 }, // 1750 ft
        {   8000,    5024,    1732 }, // 2000 ft
        {   9652,    6484,    2559 }, // 2500 ft
        {  10950,    7800,    2850 }, // 3000 ft
        {  13350,    9430,    3270 }, // 3500 ft
    };

    public override double Interpolate(double distance, double rcr)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, distance, rcr);
    }

    public override string Description => "Landing RCR correction for ground roll distance";
}
