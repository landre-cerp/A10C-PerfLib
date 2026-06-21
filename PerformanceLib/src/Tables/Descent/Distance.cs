namespace a10c_perf_lib.src.Tables.Descent;

internal sealed class DescentDistanceTable
{
    internal static readonly DescentDistanceTable Instance = new();

    private static readonly double[] XAxis = { 0, 5000, 10000, 15000, 20000, 25000, 30000, 35000 };
    private static readonly double[] YAxis = { 25000, 27500, 30000, 32500, 35000, 37500, 40000, 42500, 45000, 47500, 50000 };

    // Cumulative descent distance (NM). Rows = pressure altitude, cols = gross weight.
    private static readonly double[,] MatrixDrag0 =
    {
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, //     0
        {  0.4,  0.5,  0.6,  0.6,  0.7,  0.8,  0.8,  0.9,    1,    1,  1.1 }, //  5000
        {  0.7,  0.8,    1,  1.1,  1.2,  1.3,  1.4,  1.5,  1.6,  1.7,  1.8 }, // 10000
        {  0.8,  0.9,    1,    1,    1,    1,  0.9,  0.9,  0.8,  0.7,  0.5 }, // 15000
        {    1,  1.1,  1.1,  1.1,  1.1,  1.1,    1,  0.9,  0.7,  0.5,  0.3 }, // 20000
        {  1.3,  1.4,  1.5,  1.5,  1.5,  1.4,  1.3,  1.1,  0.9,  0.6,  0.2 }, // 25000
        {    1,    1,    1,  0.9,  0.8,  0.5,  0.1, -0.3, -0.9, -1.5, -2.2 }, // 30000
        {   42, 45.3, 48.4, 51.2, 53.6, 55.9, 57.8, 59.5, 60.9,   62, 62.8 }, // 35000
    };

    private static readonly double[,] MatrixDrag4 =
    {
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, //     0
        {  0.4,  0.5,  0.6,  0.6,  0.7,  0.8,  0.8,  0.9,  0.9,    1,  1.1 }, //  5000
        {  0.7,  0.8,  0.9,  1.1,  1.2,  1.3,  1.4,  1.6,  1.7,  1.8,  1.9 }, // 10000
        {    1,  1.1,  1.1,  1.2,  1.3,  1.3,  1.3,  1.3,  1.3,  1.2,  1.2 }, // 15000
        {    1,  1.1,  1.2,  1.2,  1.2,  1.2,  1.1,    1,  0.8,  0.6,  0.4 }, // 20000
        {  1.4,  1.6,  1.6,  1.7,  1.7,  1.6,  1.5,  1.3,  1.1,  0.9,  0.6 }, // 25000
        {  1.3,  1.4,  1.4,  1.3,  1.1,  0.9,  0.5,  0.1, -0.4,   -1, -1.7 }, // 30000
        { 40.2, 43.3, 46.1, 48.7,   51, 53.1, 54.9, 56.4, 57.7, 58.7, 59.5 }, // 35000
    };

    private static readonly double[,] MatrixDrag8 =
    {
        {    0,    0,    0,    0,    0,    0,    0,    0,    0,    0,    0 }, //     0
        {  0.5,  0.5,  0.6,  0.6,  0.7,  0.8,  0.8,  0.9,  0.9,    1,  1.1 }, //  5000
        {  0.8,  0.9,    1,  1.1,  1.3,  1.4,  1.5,  1.6,  1.8,  1.9,    2 }, // 10000
        {  1.2,  1.3,  1.4,  1.5,  1.6,  1.7,  1.7,  1.8,  1.8,  1.8,  1.8 }, // 15000
        {  1.1,  1.2,  1.2,  1.3,  1.3,  1.2,  1.2,  1.1,  0.9,  0.8,  0.6 }, // 20000
        {  1.6,  1.7,  1.8,  1.9,  1.9,  1.8,  1.7,  1.6,  1.4,  1.2,  0.9 }, // 25000
        {  1.4,  1.5,  1.5,  1.4,  1.2,    1,  0.7,  0.3, -0.1, -0.7, -1.3 }, // 30000
        { 38.4, 41.3, 43.9, 46.3, 48.4, 50.4,   52, 53.4, 54.6, 55.5, 56.2 }, // 35000
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
