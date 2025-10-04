namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Fifty-foot obstacle clearance table for flaps in TO (7°) position.
/// Max thrust
/// X-axis: Ground run distance (0-14000 ft), Y-axis: Wind speed (-20 to 40 knots)
/// </summary>
internal sealed class FiftyFtClearanceFlapsToMaxThrustTable : CorrectionTable
{
    private static readonly double[] XAxis = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    private static readonly double[] YAxis = { -20, 0, 20, 40 };
    
    private static readonly double[,] Matrix =
    {
        // -20,      0     20,    40
        {     0,     0,     0,     0 },// 0
        {  1270,  1343,  1417,  1490 },// 1000
        {  2600,  2830,  3060,  3290 },// 2000
        {  3980,  4315,  4650,  5300 },// 3000
        {  5400,  5780,  6550,  7790 },// 4000
        {  6840,  7380,  8720, 11400 },// 5000
        {  8420,  9280, 11450, 17800 },// 6000
        {  9990, 11390, 15405,    -1 },// 7000
        { 11870, 14180, 21920,    -1 },// 8000
        { 14080, 18000,    -1,    -1 },// 9000
        { 17140, 23770,    -1,    -1 },// 10000
        { 21070,    -1,    -1,    -1 },// 11000
        {    -1,    -1,    -1,    -1 },// 12000
        {    -1,    -1,    -1,    -1 },// 13000
        {    -1,    -1,    -1,    -1 } // 14000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "Fifty-feet obstacle clearance distance with flaps TO (7°), Max thrust";
}
