namespace a10c_perf_lib.src.Tables.Descent;

internal sealed class DescentFuelTable
{
    internal static readonly DescentFuelTable Instance = new();

    private static readonly double[] XAxis = { 0, 5000, 10000, 15000, 20000, 25000, 30000, 35000 };
    private static readonly double[] YAxis = { 25000, 27500, 30000, 32500, 35000, 37500, 40000, 42500, 45000, 47500, 50000 };

    // Cumulative descent fuel used (lbs). Rows = pressure altitude, cols = gross weight.
    private static readonly double[,] MatrixDrag0 =
    {
        {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 }, //     0
        {   3,   4,   4,   4,   5,   5,   5,   6,   6,   6,   7 }, //  5000
        {   6,   7,   8,   8,   9,  10,  10,  11,  12,  12,  13 }, // 10000
        {  10,  11,  12,  12,  13,  13,  14,  14,  14,  14,  14 }, // 15000
        {  16,  17,  18,  19,  19,  20,  19,  19,  19,  18,  17 }, // 20000
        {  23,  24,  25,  26,  26,  25,  25,  23,  22,  20,  18 }, // 25000
        {  29,  30,  30,  29,  28,  26,  24,  21,  17,  13,   8 }, // 30000
        { 366, 391, 414, 436, 454, 471, 485, 497, 507, 514, 519 }, // 35000
    };

    private static readonly double[,] MatrixDrag4 =
    {
        {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 }, //     0
        {   3,   3,   3,   4,   4,   4,   5,   5,   5,   6,   6 }, //  5000
        {   6,   7,   7,   8,   9,  10,  10,  11,  12,  12,  13 }, // 10000
        {  10,  10,  11,  12,  12,  13,  14,  14,  14,  15,  15 }, // 15000
        {  14,  15,  16,  16,  16,  16,  16,  16,  16,  15,  14 }, // 20000
        {  20,  21,  21,  22,  21,  21,  20,  19,  18,  16,  13 }, // 25000
        {  26,  26,  26,  26,  25,  23,  21,  19,  15,  12,   7 }, // 30000
        { 322, 345, 365, 383, 399, 414, 426, 436, 444, 450, 454 }, // 35000
    };

    private static readonly double[,] MatrixDrag8 =
    {
        {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,   0 }, //     0
        {   3,   3,   3,   3,   4,   4,   4,   4,   5,   5,   5 }, //  5000
        {   5,   6,   6,   7,   8,   8,   9,   9,  10,  11,  11 }, // 10000
        {   8,   9,  10,  10,  11,  11,  12,  12,  13,  13,  14 }, // 15000
        {  12,  13,  13,  14,  14,  14,  14,  14,  13,  13,  12 }, // 20000
        {  17,  17,  18,  18,  18,  17,  17,  16,  14,  13,  11 }, // 25000
        {  22,  23,  23,  22,  21,  20,  18,  15,  12,   9,   5 }, // 30000
        { 279, 299, 316, 332, 346, 358, 368, 377, 383, 388, 391 }, // 35000
    };

    internal double Interpolate(double altFt, double weight, double drag)
    {
        var (m0, m1, t) = DragBracket(drag);
        double v0 = CorrectionTable.BilinearInterpolate(m0, XAxis, YAxis, altFt, weight);
        double v1 = CorrectionTable.BilinearInterpolate(m1, XAxis, YAxis, altFt, weight);
        return v0 + (v1 - v0) * t;
    }

    private static (double[,] m0, double[,] m1, double t) DragBracket(double drag) =>
        drag <= 4 ? (MatrixDrag0, MatrixDrag4, drag / 4.0) : (MatrixDrag4, MatrixDrag8, (drag - 4.0) / 4.0);
}
