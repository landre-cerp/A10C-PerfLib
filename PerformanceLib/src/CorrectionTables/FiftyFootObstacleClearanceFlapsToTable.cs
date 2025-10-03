namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Fifty-foot obstacle clearance table for flaps in TO (7°) position.
/// X-axis: Ground run distance (0-14000 ft), Y-axis: Wind speed (-20 to 40 knots)
/// </summary>
internal sealed class FiftyFootObstacleClearanceFlapsToTable : CorrectionTable
{
    private static readonly double[] XAxis = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    private static readonly double[] YAxis = { -20, 0, 20, 40 };
    
    private static readonly double[,] Matrix =
    {
        // -20,      0     20,    40
        {     0,     0,     0,     0 },// 0
        {  1230,  1300,  1370,  1440 },// 1000
        {  2550,  2780,  3010,  3240 },// 2000
        {  3940,  4270,  4600,  5300 },// 3000
        {  5340,  5750,  6520,  7760 },// 4000
        {  6800,  7340,  8670, 11420 },// 5000
        {  8380,  9230, 11410, 17900 },// 6000
        {  9930, 11380, 15405,    -1 },// 7000
        { 11820, 14150, 21940,    -1 },// 8000
        { 14000, 17990,    -1,    -1 },// 9000
        { 17105, 23740,    -1,    -1 },// 10000
        { 21050,    -1,    -1,    -1 },// 11000
        {    -1,    -1,    -1,    -1 },// 12000
        {    -1,    -1,    -1,    -1 },// 13000
        {    -1,    -1,    -1,    -1 } // 14000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "Fifty-foot obstacle clearance distance with flaps TO (7°)";
}