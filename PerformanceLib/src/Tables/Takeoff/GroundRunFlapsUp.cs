
namespace a10c_perf_lib.src.Tables.Takeoff;

internal sealed class GroundRunFlapsUpTable : CorrectionTable
{
    // Gross weight (lbs)
    private static readonly double[] YAxis = { 30000, 35000, 40000, 45000, 50000 };

    private static readonly double[] XAxis = { 4, 5, 6, 7, 8, 9, 10, 11 };

    private static readonly double[,] Matrix =
    {
        // 30k, 35k, 40k, 45k, 50k
        { 4800, 7400, 11000,    -1,    -1 },// 4
        { 4260, 6420,  9300, 13350,    -1 },// 5
        { 3720, 5530,  7900, 10950,    -1 },// 6
        { 3170, 4620,  6560,  9050, 12740 },// 7
        { 2600, 3770,  5280,  7270,  9880 },// 8
        { 2070, 2910,  4050,  5570,  7340 },// 9
        { 1510, 2070,  2850,  3860,  4940 },// 10
        {  970, 1240,  1680,  2150,  2650 } // 11
    };

    public override double Interpolate(double takeoffindex, double grossweight)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, takeoffindex, grossweight);
    }

    public override string Description => "Ground distance at takeoff";
}
