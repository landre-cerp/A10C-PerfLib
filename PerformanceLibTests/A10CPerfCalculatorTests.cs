using a10c_perf_lib.src;


namespace a10c_perf_lib.Tests;

public class A10CPerfCalculatorTests
{
    private readonly A10CPerfCalculator _calc = new();

    [Theory]
    [InlineData(-40, 85.1)]
    [InlineData(-35, 85.8)]
    [InlineData(-20, 86.1)]
    [InlineData(0, 84.5)]
    [InlineData(20, 82.1)]
    [InlineData(50, 77.6)]
    public void PTFS_ReturnsExpected(double temp, double expected)
    {
        double result = _calc.PTFS(temp);
        Assert.InRange(result, expected - 0.5, expected + 0.5);
    }

    [Theory]
    [InlineData(20000, 97.34)]
    [InlineData(30000, 118.60)]
    [InlineData(40000, 137.03)]
    public void TakeOffSpeed_ReturnsExpected(double grossWeight, double expected)
    {
        double result = _calc.TakeOffSpeed(grossWeight);
        Assert.Equal(expected, result, 2); // 2 digits precision
    }

    [Theory]
    [InlineData(-30, 0, 10.8)]
    [InlineData(0, 2_000, 9.82)]
    [InlineData(10, 2_000, 9.6)]
    [InlineData(20, 4_000, 8.6)]
    [InlineData(50, 6_000, 5.0)]
    [InlineData(5, 1_000, 9.91)] // Interpolated value
    public void GetTakeoffIndex_ReturnsExpected(double tempC, double altFt, double expected)
    {
        double result = A10CPerfCalculator.GetTakeoffIndex(tempC, altFt);
        Assert.Equal(expected, result, 1); // 2 digits precision
    }
}