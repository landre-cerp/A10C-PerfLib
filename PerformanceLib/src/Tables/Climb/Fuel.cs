namespace a10c_perf_lib.src.Tables.Climb;

internal sealed class ClimbFuelTable
{
    internal static readonly ClimbFuelTable Instance = new();

    private static readonly double[] XAxis = { 25000, 30000, 35000, 40000, 45000, 50000 };
    private static readonly double[] YAxis = { 25000, 27500, 30000, 32500, 35000, 37500, 40000, 42500, 45000, 47500, 50000 };

    // Cumulative climb fuel used (lbs). Rows = gross weight, cols = pressure altitude.
    private static readonly double[,] MatrixDrag0 =
    {
        {  452,  499,  554,  622,  707,  816,  953, 1127, 1345, 1616, 1948 }, // 25000
        {  559,  630,  712,  807,  918, 1048, 1201, 1381, 1591, 1835, 2119 }, // 30000
        {  667,  764,  889, 1052, 1264, 1538, 1889, 2331, 2882, 3560, 4386 }, // 35000
        {  871, 1024, 1211, 1438, 1713, 2041, 2432, 2892, 3429, 4052, 4770 }, // 40000
        { 1109, 1331, 1604, 1939, 2348, 2845, 3443, 4156, 5000, 5992, 7149 }, // 45000
        { 1407, 1768, 2240, 2847, 3620, 4589, 5788, 7253, 9025,11144,13656 }, // 50000
    };

    private static readonly double[,] MatrixDrag4 =
    {
        {  501,  562,  634,  723,  833,  971, 1144, 1359, 1626, 1953, 2351 }, // 25000
        {  621,  711,  816,  939, 1084, 1254, 1453, 1685, 1953, 2263, 2619 }, // 30000
        {  774,  890, 1038, 1230, 1482, 1810, 2233, 2773, 3452, 4296, 5331 }, // 35000
        {  998, 1194, 1446, 1769, 2183, 2707, 3365, 4180, 5180, 6392, 7849 }, // 40000
        { 1300, 1572, 1910, 2328, 2844, 3475, 4241, 5164, 6267, 7573, 9110 }, // 45000
        { 1752, 2246, 2916, 3812, 4992, 6518, 8460,10895,13904,17576,22007 }, // 50000
    };

    private static readonly double[,] MatrixDrag8 =
    {
        {  548,  612,  689,  787,  913, 1076, 1288, 1559, 1904, 2336, 2870 }, // 25000
        { 1256, 1402, 1571, 1776, 2032, 2356, 2767, 3287, 3939, 4750, 5748 }, // 30000
        {  873, 1018, 1196, 1419, 1696, 2041, 2468, 2993, 3631, 4402, 5325 }, // 35000
        { 1187, 1494, 1913, 2477, 3225, 4197, 5440, 7004, 8943,11316,14186 }, // 40000
        { 1543, 1980, 2573, 3367, 4411, 5759, 7471, 9611,12250,15463,19332 }, // 45000
        { 1927, 2347, 2838, 3409, 4064, 4811, 5656, 6602, 7657, 8824,10109 }, // 50000
    };

    internal double Interpolate(double weight, double altFt, double drag)
    {
        if (altFt < YAxis[0]) return 0;
        var (m0, m1, t) = DragBracket(drag);
        double v0 = CorrectionTable.BilinearInterpolate(m0, XAxis, YAxis, weight, altFt);
        double v1 = CorrectionTable.BilinearInterpolate(m1, XAxis, YAxis, weight, altFt);
        return v0 + (v1 - v0) * t;
    }

    private static (double[,] m0, double[,] m1, double t) DragBracket(double drag) =>
        drag <= 4 ? (MatrixDrag0, MatrixDrag4, drag / 4.0) : (MatrixDrag4, MatrixDrag8, (drag - 4.0) / 4.0);
}
