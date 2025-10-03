
namespace a10c_perf_lib.src.CorrectionTables;

internal sealed class TakeoffGroundRunTable : CorrectionTable
{
    // Gross weight (lbs) 
    private static readonly double[] YAxis = { 30000, 35000, 40000, 45000, 50000 };

    private static readonly double[] XAxis = { 4, 4.2, 5, 6, 7, 8, 9, 10, 11 };

    private static readonly double[,] Matrix =
    {
        // 30k, 35k, 40k, 45k, 50k
        { 4500, 6800, 9900, 15000 , -1 },// 4
        { 4450, 6650, 9700, 14000, -1 },// 4.2
        { 4000, 5900, 8500, 12200, -1 },// 5
        { 3500, 5100,7250, 10200, -1 },// 6
        { 2980, 4300,6100, 8300,11700  },// 7
        { 2400, 3500,4900, 6600,9200 },// 8
        { 1900, 2750,3800, 5000,6900 },// 9
        { 1350, 1950,2650,3500, 4600},// 10
        { 800,1200,1600,2000,2500 } // 11
    };

    public override double Interpolate(double takeoffindex, double grossweight)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, takeoffindex, grossweight);
    }

    public override string Description => "Ground distance at takeoff";
}
