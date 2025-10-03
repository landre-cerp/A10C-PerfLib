using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class PTFSTests
{
    [Theory]
    [InlineData(-40, 85.1)]
    [InlineData(-35, 85.8)]
    [InlineData(-20, 86.1)]
    [InlineData(0, 84.5)]
    [InlineData(20, 82.1)]
    [InlineData(50, 77.6)]
    public void PTFS_ReturnsExpected(double temp, double expected)
    {
        double result = PTFS(temp);
        Assert.InRange(result, expected - 0.5, expected + 0.5);
    }

    [Theory]
    [InlineData(-41)]
    [InlineData(51)]
    public void PTFS_OutOfRange_Throws(double temp)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PTFS(temp));
    }
}