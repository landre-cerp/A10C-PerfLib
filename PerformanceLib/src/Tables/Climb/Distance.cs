namespace a10c_perf_lib.src.Tables.Climb;

internal sealed class ClimbDistanceTable
{
    internal static readonly ClimbDistanceTable Instance = new();

    private static readonly double[] XAxis = { 25000, 30000, 35000, 40000, 45000, 50000 };
    private static readonly double[] YAxis = { 25000, 27500, 30000, 32500, 35000, 37500, 40000, 42500, 45000, 47500, 50000 };

    // Cumulative climb distance (NM). Rows = gross weight, cols = pressure altitude.
    private static readonly double[,] MatrixDrag0 =
    {
        { -20.6, -23.9, -28.3, -34.7,  -43.8,  -56.9,  -74.9,  -99.4, -131.9, -174.1, -227.8 }, // 25000
        {  24.8,  30.7,  38.3,  47.9,   59.8,   74.7,   92.8,  114.8,  141.2,  172.6,  209.6 }, // 30000
        { -30.3,   -38, -49.8, -67.7,  -93.9, -130.8, -181.5, -249.2, -337.4, -450.1, -591.6 }, // 35000
        {  40.8,  53.8,  73.1, 100.8,  139.5,    192,  261.4,  351.4,  465.7,  608.5,  784.3 }, // 40000
        {  56.6,  80.4, 116.9, 170.6,  246.9,  351.9,  492.2,  675.4,  909.7,   1204, 1567.9 }, // 45000
        {  80.2, 115.5, 167.8, 242.8,  346.9,  487.6,  673.1,  912.4, 1215.3, 1592.6,   2056 }, // 50000
    };

    private static readonly double[,] MatrixDrag4 =
    {
        { -21.4, -25.7, -31.8, -40.4,  -52.5,    -69,    -91,   -120, -157.2, -204.3, -262.9 }, // 25000
        {  26.9,  33.9,  43.3,    56,   72.6,   94.3,  121.9,  156.6,  199.6,  252.2,  315.7 }, // 30000
        {  35.2,  46.3,  62.5,  85.5,  117.1,  159.7,  215.6,  287.4,  378.2,    491,  629.2 }, // 35000
        {  47.3,  63.2,  86.2, 118.4,  162.3,  220.7,  296.8,    394,  515.9,  666.6,  850.5 }, // 40000
        {  65.2,  85.2, 110.6, 142.2,  181.2,  228.6,  285.4,    353,  432.4,  525.1,  632.3 }, // 45000
        {  97.3, 139.2, 197.8, 277.5,  383.2,  520.4,  694.9,  913.3, 1182.4, 1509.9, 1903.6 }, // 50000
    };

    private static readonly double[,] MatrixDrag8 =
    {
        {  24.2,  29.8,  36.4,  44.4,   53.9,   65.1,   78.2,   93.4,  111.1,  131.3,  154.5 }, // 25000
        {  30.1,  38.6,  50.9,  68.4,   92.4,  124.7,  167.2,  221.9,  291.2,  377.4,  483.3 }, // 30000
        {  41.5,  54.6,  72.2,  95.2,    125,  162.8,  210.2,  268.7,  340.1,  426.1,  528.7 }, // 35000
        {  57.4,  83.7,   124, 182.9,  265.8,  378.6,  527.9,  721.1,    966, 1271.2,   1646 }, // 40000
        {  80.2, 111.6, 155.1, 213.8,    291,  390.6,  516.9,  674.3,  867.8, 1102.5, 1384.3 }, // 45000
        {  99.9, 128.4, 162.4, 202.2,  248.3,  301.1,  360.8,  427.7,  502.2,  584.4,  674.6 }, // 50000
    };

    internal double Interpolate(double weight, double altFt, double drag)
    {
        if (altFt < YAxis[0]) return 0;
        var (m0, m1, t) = DragBracket(drag);
        double v0 = CorrectionTable.BilinearInterpolate(m0, XAxis, YAxis, weight, altFt);
        double v1 = CorrectionTable.BilinearInterpolate(m1, XAxis, YAxis, weight, altFt);
        return v0 + (v1 - v0) * t;
    }

    private static (double[,] m0, double[,] m1, double t) DragBracket(double drag) =>
        drag <= 4 ? (MatrixDrag0, MatrixDrag4, drag / 4.0) : (MatrixDrag4, MatrixDrag8, (drag - 4.0) / 4.0);
}
