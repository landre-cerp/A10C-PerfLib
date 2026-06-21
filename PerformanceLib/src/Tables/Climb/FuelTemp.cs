namespace a10c_perf_lib.src.Tables.Climb;

/// <summary>
/// Temperature correction for climb fuel (lbs).
/// X-axis: base fuel (lbs), Y-axis: deltaISA (°C). At Y=0 output equals input.
/// </summary>
internal sealed class ClimbFuelTempTable : CorrectionTable
{
    // Base fuel brackets (keys from TS PosNegCorrectionTable positive side)
    private static readonly double[] XAxis = { 250, 500, 750, 1000, 1250, 1500, 1750, 2000 };
    private static readonly double[] YAxis = { -40, -20, 0, 20, 40, 60 };

    // TS polynomial: f(deltaISA) = c0 + c1*deltaISA  (positive side only)
    // Keys:                    250    500    750    1000   1250   1500   1750   2000
    // c0:                      250    500    752    1018   1254   1495   1768   2000
    // c1:                      2.5    5.0    7.5    12.5   21.3   27.5   39.5   55.0
    private static readonly double[,] Matrix =
    {
        //   -40    -20      0      20      40      60
        {    150,   200,   250,   300,    350,    400  }, // base = 250
        {    300,   400,   500,   600,    700,    800  }, // base = 500
        {    452,   602,   752,   902,   1052,   1202  }, // base = 750
        {    518,   768,  1018,  1268,   1518,   1768  }, // base = 1000
        {    402,   828,  1254,  1680,   2106,   2532  }, // base = 1250
        {    395,   945,  1495,  2045,   2595,   3145  }, // base = 1500
        {    188,   978,  1768,  2558,   3348,   4138  }, // base = 1750
        {   -200,   900,  2000,  3100,   4200,   5300  }, // base = 2000
    };

    public override double Interpolate(double baseFuelLbs, double deltaISA)
        => BilinearInterpolate(Matrix, XAxis, YAxis, baseFuelLbs, deltaISA);

    public override string Description => "Climb fuel temperature correction";
}
