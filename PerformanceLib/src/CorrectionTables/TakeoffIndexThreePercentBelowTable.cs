namespace a10c_perf_lib.src.CorrectionTables;

/// <summary>
/// Takeoff index table for 3% below maximum thrust setting.
/// X-axis: Temperature (-30 to 50°C), Y-axis: Altitude (0-5000 ft in thousands)
/// </summary>
internal sealed class TakeoffIndexThreePercentBelowTable : CorrectionTable
{
    private static readonly double[] XAxis = { -30, -20, -10, 0, 10, 20, 30, 40, 50 };
    private static readonly double[] YAxis = { 0, 2000, 4000, 5000 }; // Note: Limited to 5000 ft for 3% below

    private static readonly double[,] Matrix =
    {
        { 10.71, 10.40, 9.90, 9.41 },// -30°C
        { 10.41, 10.00, 9.62, 9.18 },
        { 10.21,  9.80, 9.40, 8.70 },
        { 10.00,  9.40, 8.90, 8.20 },
        {  9.60,  9.00, 8.41, 7.62 },
        {  9.25,  8.78, 7.60, 6.85 },
        {  8.80,  7.95, 7.00, 5.90 },
        {  8.35,  7.20, 5.95, 4.70 },
        {  7.20,  6.18, 4.80, 4.00} // 50°C
    };

    public override double Interpolate(double temp, double altitude)
    {
        return BilinearInterpolate(Matrix, XAxis, YAxis, temp, altitude);
    }


    public override string Description => "Takeoff index calculation for 3% below maximum thrust";
}