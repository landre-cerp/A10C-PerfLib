namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Takeoff index table for maximum thrust setting.
/// X-axis: Temperature (-30 to 50°C), Y-axis: Altitude (0-6000 ft in thousands)
/// </summary>
internal sealed class TakeoffIndexMaxThrustTable : CorrectionTable
{
    private static readonly double[] XAxis = { -30, -20, -10, 0, 10, 20, 30, 40, 50 };
    private static readonly double[] YAxis = { 0, 2000, 4000, 6000 };

    private static readonly double[,] Matrix =
    {
        { 10.80, 10.60, 10.22, 9.82 },// -30°C
        { 10.70, 10.40, 10.10, 9.60 },
        { 10.50, 10.20,  9.80, 9.37 },
        { 10.25,  9.82,  9.50, 8.90 },
        { 10.05,  9.60,  9.10, 8.40 },
        {  9.75,  9.20,  8.60, 7.82 },
        {  9.40,  8.70,  7.90, 7.10 },
        {  8.80,  8.10,  7.20, 6.10 },
        {  8.10,  7.21,  6.21, 5.00 } // 50°C
    };

    public override double Interpolate(double temp, double altitude)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, temp, altitude);
    }

    public override string Description => "Takeoff index calculation for maximum thrust";
}