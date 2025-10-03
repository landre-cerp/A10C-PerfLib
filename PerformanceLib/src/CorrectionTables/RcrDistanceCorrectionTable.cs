namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// RCR (Runway Condition Reading) distance correction table.
/// X-axis: Distance (1000-12000 ft), Y-axis: RCR values (5-23)
/// </summary>
internal sealed class RcrDistanceCorrectionTable : CorrectionTable
{
    private static readonly double[] XAxis = { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000, 11000, 12000 };
    private static readonly double[] YAxis = { 5, 10, 15, 20, 23 };
    
    private static readonly double[,] Matrix =
    {
        // 5,     10,    15,    20,    23
        { 1380,  1160,  1030,  1000,  1000}, // 1000
        { 2820,  2370,  2130,  2000,  2000}, // 2000
        { 4560,  3800,  3320,  3100,  3000}, // 3000
        { 6400,  5250,  4600,  4200,  4000}, // 4000
        { 8250,  6800,  5800,  5250,  5000}, // 5000
        {10200,  8200,  7150,  6300,  6000}, // 6000
        {12000,  9770,  8500,  7500,  7000}, // 7000
        {   -1, 11300,  9700,  8500,  8000}, // 8000
        {   -1,    -1, 10900,  9600,  9000}, // 9000
        {   -1,    -1,    -1, 10900, 10000}, // 10000
        {   -1,    -1,    -1, 12000, 11000}, // 11000
        {   -1,    -1,    -1,    -1, 12000}  // 12000
    };

    public override double Interpolate(double x, double y)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, x, y);
    }

    public override string Description => "RCR distance correction for runway conditions";
}