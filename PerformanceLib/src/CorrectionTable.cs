namespace a10c_perf_lib.src;

/// <summary>
/// Abstract base class for correction tables that perform bilinear interpolation.
/// Each concrete implementation encapsulates its own matrix and axis data.
/// </summary>
public abstract class CorrectionTable
{
    /// <summary>
    /// Performs bilinear interpolation for the given x and y values.
    /// </summary>
    /// <param name="x">X value to interpolate</param>
    /// <param name="y">Y value to interpolate</param>
    /// <returns>Interpolated value</returns>
    public abstract double Interpolate(double x, double y);

    /// <summary>
    /// Gets a description of what this table represents.
    /// </summary>
    public abstract string Description { get; }

    /// <summary>
    /// Protected helper method for bilinear interpolation that concrete classes can use.
    /// </summary>
    protected static double BilinearInterpolate(
        double[,] matrix,
        double[] xAxis,
        double[] yAxis,
        double x,
        double y)
    {
        if (!IsInRange(xAxis, x))
            throw new ArgumentOutOfRangeException(nameof(x), $"X value {x} is out of range ({xAxis[0]} to {xAxis[^1]})");
        if (!IsInRange(yAxis, y))
            throw new ArgumentOutOfRangeException(nameof(y), $"Y value {y} is out of range ({yAxis[0]} to {yAxis[^1]})");

        int i = FindInterval(xAxis, x);
        int j = FindInterval(yAxis, y);

        double x1 = xAxis[i], x2 = xAxis[i + 1];
        double y1 = yAxis[j], y2 = yAxis[j + 1];

        double Q11 = matrix[i, j];
        double Q21 = matrix[i + 1, j];
        double Q12 = matrix[i, j + 1];
        double Q22 = matrix[i + 1, j + 1];

        double t = (x - x1) / (x2 - x1);
        double u = (y - y1) / (y2 - y1);

        return (1 - t) * (1 - u) * Q11 +
               t * (1 - u) * Q21 +
               (1 - t) * u * Q12 +
               t * u * Q22;
    }

    private static int FindInterval(double[] arr, double val)
    {
        for (int i = 0; i < arr.Length - 1; i++)
            if (val >= arr[i] && val <= arr[i + 1])
                return i;
        return arr.Length - 2; 
    }

    private static bool IsInRange(double[] axis, double value) =>
        value >= axis[0] && value <= axis[^1];
}