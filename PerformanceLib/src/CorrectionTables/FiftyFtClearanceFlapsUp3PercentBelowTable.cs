namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Fifty-feet obstacle clearance table for flaps in UP (0°) position.
/// Thrust 3 percent below PTFS
/// X-axis: Ground run distance (0-14000 ft), Y-axis: Wind speed (-20 to 40 knots)
/// </summary>
internal sealed class FiftyFtClearanceFlapsUp3PercentBelowTable : CorrectionTable
{
    private static readonly double[] XAxis = { 0, 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000, 13000, 14000 };
    private static readonly double[] YAxis = { -20, 0, 20, 40 };

    private static readonly double[,] Matrix =
    {
        // -20,     0     20,    40
        {     0,     0,     0,     0 },// 0
        {  1280,  1393,  1507,  1620 },// 1000
        {  2590,  2860,  3123,  3390 },// 2000
        {  3920,  4260,  4600,  5250 },// 3000
        {  5290,  5720,  6370,  7430 },// 4000
        {  6690,  7320,  8360, 10110 },// 5000
        {  8230,  9110, 10660, 13950 },// 6000
        {  9820, 11000, 13290, 20910 },// 7000
        { 11640, 13220, 16820,    -1 },// 8000
        { 13540, 15880, 22430,    -1 },// 9000
        { 15730, 19120,    -1,    -1 },// 10000
        { 18380, 23620,    -1,    -1 },// 11000
        { 21580,    -1,    -1,    -1 },// 12000
        {    -1,    -1,    -1,    -1 },// 13000
        {    -1,    -1,    -1,    -1 } // 14000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "Fifty-feet obstacle clearance distance with flaps UP (0°), 3% Percent below PTFS";
}