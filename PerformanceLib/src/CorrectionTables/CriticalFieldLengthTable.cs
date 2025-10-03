namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Critical field length table.
/// X-axis: Takeoff index (4-11), Y-axis: Gross weight (30000-50000 lbs)
/// </summary>
internal sealed class CriticalFieldLengthTable : CorrectionTable
{
    private static readonly double[] XAxis = { 4, 5, 6, 7, 8, 9, 10, 11 };
    private static readonly double[] YAxis = { 30000, 35000, 40000, 45000, 50000 };
    
    private static readonly double[,] Matrix =
    {
        // 30000, 35000, 40000, 45000, 50000
        { 6500,   -1,   -1,   -1,    -1 },// 4
        { 5860, 8640,   -1,   -1,    -1 },// 5
        { 5185, 7430,   -1,   -1,    -1 },// 6
        { 4500, 6300, 8700,   -1,    -1 },// 7
        { 3800, 5270, 7175, 9845,    -1 },// 8
        { 3100, 4230, 5700, 7530, 10180 },// 9
        { 2400, 3200, 4200, 5340,  6920 },// 10
        { 1670, 2200, 2740, 3260,  3920 } // 11
    };

    public override double Interpolate(double takeoffIndex, double grossWeight)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, takeoffIndex, grossWeight);
    }

    public override string Description => "Critical field length for takeoff calculations";
}