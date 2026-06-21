namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Landing ground roll table, flaps 20°, speedbrakes 100% open.
/// X-axis: Landing Index (70–120), Y-axis: Gross weight (lbs)
/// Data computed from TS polynomial coefficients in Landing.ts (vector_landing_distance_flaps20_SB100).
/// </summary>
internal sealed class LandingGroundRollFlaps20SB100Table : CorrectionTable
{
    private static readonly double[] XAxis = { 70, 75, 80, 85, 90, 95, 100, 105, 110, 115, 120 };
    private static readonly double[] YAxis = { 25000, 30000, 35000, 40000, 45000, 50000 };

    private static readonly double[,] Matrix =
    {
        // 25k    30k    35k    40k    45k    50k
        { 1275, 1400,  1514,  1649,  1794,  1944 }, // LI=70
        { 1236, 1352,  1460,  1589,  1723,  1871 }, // LI=75
        { 1201, 1307,  1410,  1532,  1658,  1801 }, // LI=80
        { 1169, 1267,  1363,  1479,  1596,  1736 }, // LI=85
        { 1139, 1230,  1321,  1432,  1539,  1675 }, // LI=90
        { 1113, 1197,  1283,  1388,  1488,  1619 }, // LI=95
        { 1089, 1167,  1249,  1348,  1439,  1567 }, // LI=100
        { 1068, 1141,  1219,  1312,  1396,  1519 }, // LI=105
        { 1051, 1119,  1193,  1281,  1357,  1476 }, // LI=110
        { 1036, 1100,  1171,  1254,  1322,  1437 }, // LI=115
        { 1024, 1085,  1153,  1231,  1292,  1402 }, // LI=120
    };

    public override double Interpolate(double landingIndex, double grossWeight)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, landingIndex, grossWeight);
    }

    public override string Description => "Landing ground roll, flaps 20°, speedbrakes 100%";
}
