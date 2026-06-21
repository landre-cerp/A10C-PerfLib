namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Wind correction table for landing ground roll, flaps 20°.
/// X-axis: Base distance (ft), Y-axis: Headwind component (knots, negative = tailwind)
/// Data evaluated from TS PosNegCorrectionTable polynomials in Landing.ts (headwind_landing_distance_flaps20).
/// </summary>
internal sealed class LandingWindCorrectionTable : CorrectionTable
{
    private static readonly double[] XAxis = { 1000, 1500, 2000, 2500, 3000, 3500, 4000 };
    private static readonly double[] YAxis = { -20, -15, -10, -5, 0, 5, 10, 15, 20, 25, 30 };

    private static readonly double[,] Matrix =
    {
        // -20   -15   -10   -5    0     5     10    15    20    25    30
        { 1600, 1450, 1300, 1150, 1000,  900,  800,  700,  600,  500,  400 }, // 1000 ft
        { 2200, 2025, 1850, 1675, 1488, 1398, 1303, 1200, 1092,  977,  857 }, // 1500 ft
        { 2800, 2600, 2400, 2200, 2028, 1912, 1796, 1677, 1558, 1437, 1316 }, // 2000 ft
        { 3300, 3100, 2900, 2700, 2488, 2373, 2253, 2125, 1992, 1852, 1707 }, // 2500 ft
        { 4000, 3750, 3500, 3250, 3013, 2883, 2748, 2605, 2457, 2302, 2142 }, // 3000 ft
        { 4600, 4325, 4050, 3775, 3425, 3316, 3195, 3062, 2915, 2756, 2585 }, // 3500 ft
        { 5200, 4900, 4600, 4300, 3925, 3816, 3695, 3561, 3415, 3256, 3085 }, // 4000 ft
    };

    public override double Interpolate(double distance, double windKnots)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, distance, windKnots);
    }

    public override string Description => "Landing wind correction for ground roll distance";
}
