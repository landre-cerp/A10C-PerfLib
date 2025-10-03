using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class TakeoffSpeedTests
{
    [Theory]
    [InlineData(EMPTY_WEIGHT_LBS, 109.69)]
    [InlineData(30000, 118.60)]
    [InlineData(40000, 137.03)]
    [InlineData(MAX_TAKEOFF_WEIGHT_LBS, 147.77)]
    public void TakeOffSpeed_ReturnsExpected(double grossWeight, double expected)
    {
        double result = TakeOffSpeed(grossWeight);
        Assert.Equal(expected, result, 2); 
    }

    [Theory]
    [InlineData(EMPTY_WEIGHT_LBS - 1)]
    [InlineData(MAX_TAKEOFF_WEIGHT_LBS + 1 )]
    public void TakeOffSpeed_OutOfRange_Throws(double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TakeOffSpeed(grossWeight));
    }

    [Fact]
    public void RotationSpeed_ReturnsExpected()
    {
        double grossWeight = 30000;
        double expectedTakeoffSpeed = 118.60; 
        
        double takeoffSpeedDefault = TakeOffSpeed(grossWeight);
        double takeoffSpeedWithFlaps = TakeOffSpeed(grossWeight, FLAPS.TO);
        double takeoffSpeedWithFlapsUp = TakeOffSpeed(grossWeight, FLAPS.UP);
        Assert.Equal(takeoffSpeedDefault, takeoffSpeedWithFlaps);
        Assert.InRange(takeoffSpeedWithFlapsUp - takeoffSpeedWithFlaps, 4, 5);

        double result = RotationSpeed(grossWeight);
        Assert.Equal(expectedTakeoffSpeed - 10, result,2);
    }
}