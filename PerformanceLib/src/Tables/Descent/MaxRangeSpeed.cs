namespace a10c_perf_lib.src.Tables.Descent;

/// <summary>
/// Max-range descent speed (KIAS).
/// X-axis: gross weight (lbs), Y-axis: drag index [0, 4, 8].
/// </summary>
internal sealed class MaxRangeSpeedTable : CorrectionTable
{
    private static readonly double[] XAxis = { 25000, 30000, 35000, 40000, 45000, 50000 };
    private static readonly double[] YAxis = { 0, 4, 8 };

    // Values from TS: poly_at_weight[w].calc(drag) = c0 + c1*drag + c2*drag²
    private static readonly double[,] Matrix =
    {
        // drag=0    drag=4    drag=8
        { 135.00,  131.60,  127.20 }, // 25000 lbs
        { 147.00,  143.20,  139.40 }, // 30000 lbs
        { 159.00,  155.00,  151.00 }, // 35000 lbs
        { 169.00,  164.69,  160.88 }, // 40000 lbs
        { 179.00,  174.38,  170.76 }, // 45000 lbs
        { 190.00,  185.00,  180.00 }, // 50000 lbs
    };

    public override double Interpolate(double grossWeight, double drag)
        => BilinearInterpolate(Matrix, XAxis, YAxis, grossWeight, drag);

    public override string Description => "Max-range penetration descent speed";
}
