namespace a10c_perf_lib.src.Tables.Climb;

/// <summary>
/// Temperature correction for climb distance (NM).
/// X-axis: base distance (NM), Y-axis: deltaISA (°C). At Y=0 output equals input.
/// </summary>
internal sealed class ClimbDistanceTempTable : CorrectionTable
{
    // Base NM values (keys from TS PosNegCorrectionTable positive side)
    private static readonly double[] XAxis = { 0, 25, 50, 75, 100, 125 };
    // deltaISA range: negative = cold day (less distance), positive = hot day (more distance)
    private static readonly double[] YAxis = { -40, -20, 0, 20, 40, 60 };

    // TS positive-side polynomial: f(deltaISA) = c0 + c1*deltaISA
    // c0 per base value: [0, 25, 50, 75, 100, 125]
    // c1 per base value: [0, 0.25, 1.1, 2.0, 3.6, 5.5]
    private static readonly double[,] Matrix =
    {
        //  -40    -20      0     20      40      60
        {    0,     0,      0,    0,      0,      0   }, // base = 0
        {   15,    20,     25,   30,     35,     40   }, // base = 25
        {    6,    28,     50,   72,     94,    116   }, // base = 50
        {   -5,    35,     75,  115,    155,    195   }, // base = 75
        {  -44,    28,    100,  172,    244,    316   }, // base = 100
        {  -95,    15,    125,  235,    345,    455   }, // base = 125
    };

    public override double Interpolate(double baseNm, double deltaISA)
        => BilinearInterpolate(Matrix, XAxis, YAxis, baseNm, deltaISA);

    public override string Description => "Climb distance temperature correction";
}
