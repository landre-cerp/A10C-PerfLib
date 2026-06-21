namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Landing index table derived from A10C-1 Chart A8-5.
/// X-axis: Temperature (°C), Y-axis: Pressure altitude (ft)
/// Data computed from TS polynomial coefficients in Landing.ts (vector_landingIndex).
/// </summary>
internal sealed class LandingIndexTable : CorrectionTable
{
    private static readonly double[] XAxis = { -30, -20, -10, 0, 10, 20, 30, 40, 50 };
    private static readonly double[] YAxis = { 0, 2000, 4000, 6000 };

    private static readonly double[,] Matrix =
    {
        //    0 ft   2000 ft  4000 ft  6000 ft
        { 118.82, 110.02, 102.42,  95.01 }, // -30°C
        { 114.29, 105.85,  98.36,  91.28 }, // -20°C
        { 110.02, 101.91,  94.55,  87.77 }, // -10°C
        { 106.00,  98.20,  91.00,  84.50 }, //   0°C
        { 102.24,  94.73,  87.71,  81.45 }, //  10°C
        {  98.73,  91.49,  84.68,  78.64 }, //  20°C
        {  95.48,  88.48,  81.90,  76.05 }, //  30°C
        {  92.49,  85.71,  79.38,  73.68 }, //  40°C
        {  89.75,  83.18,  77.13,  71.55 }, //  50°C
    };

    public override double Interpolate(double temp, double altitude)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, temp, altitude);
    }

    public override string Description => "Landing index from temperature and pressure altitude";
}
