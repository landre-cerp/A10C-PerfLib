using a10c_perf_lib.src;

namespace a10c_perf_lib.Tests;

public class TakeoffSpeedTests
{
    private readonly PerfCalculator _calc = new();

    [Theory]
    [InlineData(PerfCalculator.EMPTY_WEIGHT_LBS, 109.69)]
    [InlineData(30000, 118.60)]
    [InlineData(40000, 137.03)]
    [InlineData(PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS, 147.77)]
    public void TakeOffSpeed_ReturnsExpected(double grossWeight, double expected)
    {
        double result = _calc.TakeOffSpeed(grossWeight);
        Assert.Equal(expected, result, 2); 
    }

    [Theory]
    [InlineData(PerfCalculator.EMPTY_WEIGHT_LBS - 1)]
    [InlineData(PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS + 1 )]
    public void TakeOffSpeed_OutOfRange_Throws(double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calc.TakeOffSpeed(grossWeight));
    }
}