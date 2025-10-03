
namespace a10c_perf_lib.src.CorrectionTables;

internal sealed class TakeoffGroundRunTableFlapsTO : CorrectionTable
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
        { 3500, 5100, 7250, 10200, -1 },// 6
        { 2980, 4300, 6100, 8300, 11700  },// 7
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



internal sealed class TakeoffGroundRunTableFlapsUp : CorrectionTable
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


