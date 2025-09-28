namespace a10c_perf_lib;

public partial class A10CPerfCalculator
{
    // Données pour l'interpolation du Takeoff Index
    internal static readonly double[,] TakeOffIndexMaxThrust =
    {
        { 10.80, 10.60, 10.22, 9.82 },
        { 10.70, 10.40, 10.10, 9.60 },
        { 10.50, 10.20,  9.80, 9.37 },
        { 10.25,  9.82,  9.50, 8.90 },
        { 10.05,  9.60,  9.10, 8.40 },
        {  9.75,  9.20,  8.60, 7.82 },
        {  9.40,  8.70,  7.90, 7.10 },
        {  8.80,  8.10,  7.20, 6.10 },
        {  8.10,  7.21,  6.21, 5.00 }
    };

    internal static readonly double[] Temps = { -30, -20, -10, 0, 10, 20, 30, 40, 50 };
    internal static readonly double[] Alts = { 0, 2, 4, 6 };
}