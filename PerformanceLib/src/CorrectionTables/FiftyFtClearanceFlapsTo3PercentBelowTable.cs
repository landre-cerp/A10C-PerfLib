namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Fifty-feet obstacle clearance table for flaps in TO (7°) position.
/// Thrust 3 percent below PTFS
/// X-axis: Ground run distance (0-14000 ft), Y-axis: Wind speed (-20 to 40 knots)
/// </summary>
internal sealed class FiftyFtClearanceFlapsTo3PercentBelowTable : CorrectionTable
{
    private static readonly double[] XAxis = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    private static readonly double[] YAxis = { -20, 0, 20, 40 };

    private static readonly double[,] Matrix =
    {
        //  -20,     0     20,    40
        {     0,     0,     0,     0 },// 0
        {  1190,  1288,  1386,  1484 },// 1000
        {  2500,  2730,  2960,  3290 },// 2000
        {  3870,  4200,  4700,  5430 },// 3000
        {  5300,  5850,  6740,  8200 },// 4000
        {  6790,  7560,  9010, 12240 },// 5000
        {  8440,  9620, 12155, 20970 },// 6000
        { 10250, 12010, 16890,    -1 },// 7000
        { 12210, 15040,    -1,    -1 },// 8000
        { 14500, 19040,    -1,    -1 },// 9000
        { 17390,    -1,    -1,    -1 },// 10000
        { 20920,    -1,    -1,    -1 },// 11000
        {    -1,    -1,    -1,    -1 },// 12000
        {    -1,    -1,    -1,    -1 },// 13000
        {    -1,    -1,    -1,    -1 } // 14000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "Fifty-feet obstacle clearance distance with flaps TO (7°), 3% below the maximum thrust.";
}