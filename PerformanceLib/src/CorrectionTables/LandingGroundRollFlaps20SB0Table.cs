namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Landing ground roll table, flaps 20°, speedbrakes closed (0%).
/// X-axis: Landing Index (70–120), Y-axis: Gross weight (lbs)
/// Data computed from TS polynomial coefficients in Landing.ts (vector_landing_distance_flaps20_SB0).
/// </summary>
internal sealed class LandingGroundRollFlaps20SB0Table : CorrectionTable
{
    private static readonly double[] XAxis = { 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120 };
    private static readonly double[] YAxis = { 25000, 30000, 35000, 40000, 45000, 50000 };

    private static readonly double[,] Matrix =
    {
        // 25k    30k    35k    40k    45k    50k
        { 1906, 2144, 2446, 2800, 3150, 3543 }, // LI=70
        { 1822, 2053, 2324, 2644, 2987, 3348 }, // LI=75
        { 1743, 1967, 2210, 2500, 2832, 3168 }, // LI=80
        { 1667, 1884, 2105, 2369, 2688, 3003 }, // LI=85
        { 1595, 1805, 2008, 2250, 2554, 2853 }, // LI=90
        { 1528, 1731, 1920, 2144, 2429, 2717 }, // LI=95
        { 1464, 1660, 1840, 2050, 2314, 2596 }, // LI=100
        { 1404, 1593, 1769, 1969, 2209, 2490 }, // LI=105
        { 1349, 1531, 1706, 1900, 2114, 2399 }, // LI=110
        { 1297, 1472, 1652, 1844, 2028, 2322 }, // LI=115
        { 1250, 1418, 1606, 1800, 1952, 2260 }, // LI=120
    };

    public override double Interpolate(double landingIndex, double grossWeight)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, landingIndex, grossWeight);
    }

    public override string Description => "Landing ground roll, flaps 20°, speedbrakes closed";
}
