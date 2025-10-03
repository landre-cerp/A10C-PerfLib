namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Ground wind correction table for distance calculations.
/// X-axis: Distance (1000-14000 ft), Y-axis: Wind speed (-20 to 40 knots)
/// </summary>
internal sealed class GroundWindCorrectionTable : CorrectionTable
{
    private static readonly double[] XAxis = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    private static readonly double[] YAxis = { -20, -10, 0, 10, 20, 30, 40 };
    
    private static readonly double[,] Matrix =
    {
        // -20, -10, 0, 10, 20, 30, 40
        {  1400,  1200,  1000,   800,   700,  550,  400 }, // 1000
        {  2630,  2300,  2000,  1700,  1450, 1190,  886 }, // 2000
        {  3870,  3410,  3000,  2600,  2235, 1860, 1530 }, // 3000
        {  5170,  4560,  4000,  3465,  2970, 2520, 2090 }, // 4000
        {  6430,  5700,  5000,  4350,  3770, 3215, 2690 }, // 5000
        {  7630,  6795,  6000,  5245,  4560, 3900, 3300 }, // 6000
        {  8790,  7880,  7000,  6080,  5270, 4550, 3890 }, // 7000
        { 10160,  9070,  8000,  6990,  6090, 5990, 4510 }, // 8000
        { 11320, 10140,  9000,  7890,  6890, 5990, 5150 }, // 9000
        { 12600, 11320, 10000,  8800,  7660, 6650, 5730 }, // 10000
        { 13800, 12380, 11000,  9750,  8570, 7465, 6425 }, // 11000
        {    -1, 13480, 12000, 10640,  9370, 8180, 7100 }, // 12000
        {    -1,    -1, 13000, 11590, 10180, 8900, 7710 }, // 13000
        {    -1,    -1, 14000, 12430, 10980, 9620, 8400 }, // 14000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "Ground wind correction for distance calculations";
}