namespace a10c_perf_lib.src.Tables.Climb;

/// <summary>
/// Temperature correction for climb time (minutes).
/// X-axis: base time (min), Y-axis: deltaISA (°C). At Y=0 output equals input.
/// </summary>
internal sealed class ClimbTimeTempTable : CorrectionTable
{
    private static readonly double[] XAxis = { 0, 5, 10, 15, 20, 25, 30, 35 };
    private static readonly double[] YAxis = { -40, -20, 0, 20, 40, 60 };

    // TS polynomial: f(deltaISA) = c0 + c1*deltaISA  (positive side only)
    // Keys:   0    5    10   15    20    25    30    35
    // c0:     0    5    10   15    20    25    30    35
    // c1:     0   0.1  0.2  0.3  0.45  0.65  0.85  1.1
    private static readonly double[,] Matrix =
    {
        //  -40  -20   0   20   40   60
        {    0,    0,   0,   0,   0,   0  }, // base = 0
        {    1,    3,   5,   7,   9,  11  }, // base = 5
        {    2,    6,  10,  14,  18,  22  }, // base = 10
        {    3,    9,  15,  21,  27,  33  }, // base = 15
        {    2,   11,  20,  29,  38,  47  }, // base = 20
        {   -1,   12,  25,  38,  51,  64  }, // base = 25
        {   -4,   13,  30,  47,  64,  81  }, // base = 30
        {   -9,   13,  35,  57,  79, 101  }, // base = 35
    };

    public override double Interpolate(double baseTimeMin, double deltaISA)
        => BilinearInterpolate(Matrix, XAxis, YAxis, baseTimeMin, deltaISA);

    public override string Description => "Climb time temperature correction";
}
