namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Fifty-foot obstacle clearance table for flaps in UP (0°) position.
/// X-axis: Ground run distance (0-14000 ft), Y-axis: Wind speed (-20 to 40 knots)
/// </summary>
internal sealed class FiftyFootObstacleClearanceFlapsUpTable : CorrectionTable
{
    private static readonly double[] XAxis = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    private static readonly double[] YAxis = { -20, 0, 20, 40 };
    
    // FAKE DATA to be replaced with real data later.
    private static readonly double[,] Matrix =
    {
        // -20,     0     20,    40
        {     0,     0,     0,     0 },// 0
        {  1110,  1200,  1290,  1380 },// 1000
        {  2436,  2639,  2842,  3045 },// 2000
        {  3670,  4000,  4330,  4950 },// 3000
        {  5100,  5580,  6020,  7715 },// 4000
        {  6500,  7105,  7930,  9960 },// 5000
        {  8030,  8770, 10110, 13130 },// 6000
        {  9560, 10680, 12710, 18250 },// 7000
        { 11250, 12900, 16200,    -1 },// 8000
        { 13100, 15480, 20830,    -1 },// 9000
        { 15200, 18800,    -1,    -1 },// 10000
        { 17650, 22650,    -1,    -1 },// 11000
        { 20650,    -1,    -1,    -1 },// 12000
        { 24030,    -1,    -1,    -1 },// 13000
        {    -1,    -1,    -1,    -1 } // 14000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "Fifty-foot obstacle clearance distance with flaps UP (0°)";
}