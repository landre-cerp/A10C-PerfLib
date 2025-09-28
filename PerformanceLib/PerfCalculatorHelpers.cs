namespace perf_lib;

internal static class PerfCalculatorHelpers
{

    public static double BilinearInterpolate(
        double[,] matrix,
        double[] xAxis,
        double[] yAxis,
        double x,
        double y)
    {
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
        return arr.Length - 2; // extrapolation simple
    }
}