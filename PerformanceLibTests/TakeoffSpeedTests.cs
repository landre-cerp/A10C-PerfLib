using a10c_perf_lib.src;

namespace a10c_perf_lib.Tests;

public class TakeoffSpeedTests
{
    [Theory]
    [InlineData(PerfCalculator.EMPTY_WEIGHT_LBS, 109.69)]
    [InlineData(30000, 118.60)]
    [InlineData(40000, 137.03)]
    [InlineData(PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS, 147.77)]
    public void TakeOffSpeed_ReturnsExpected(double grossWeight, double expected)
    {
        double result = PerfCalculator.TakeOffSpeed(grossWeight);
        Assert.Equal(expected, result, 2); 
    }

    [Theory]
    [InlineData(PerfCalculator.EMPTY_WEIGHT_LBS - 1)]
    [InlineData(PerfCalculator.MAX_TAKEOFF_WEIGHT_LBS + 1 )]
    public void TakeOffSpeed_OutOfRange_Throws(double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PerfCalculator.TakeOffSpeed(grossWeight));
    }

    [Fact]
    public void RotationSpeed_ReturnsExpected()
    {
        double grossWeight = 30000;
        double expectedTakeoffSpeed = 118.60; 
        
        double takeoffSpeedDefault = PerfCalculator.TakeOffSpeed(grossWeight);
        double takeoffSpeedWithFlaps = PerfCalculator.TakeOffSpeed(grossWeight, PerfCalculator.FLAPS.TO);
        double takeoffSpeedWithFlapsUp = PerfCalculator.TakeOffSpeed(grossWeight, PerfCalculator.FLAPS.UP);
        Assert.Equal(takeoffSpeedDefault, takeoffSpeedWithFlaps);
        Assert.InRange(takeoffSpeedWithFlapsUp - takeoffSpeedWithFlaps, 4, 5);

        double result = PerfCalculator.RotationSpeed(grossWeight);
        Assert.Equal(expectedTakeoffSpeed - 10, result,2);
    }
}