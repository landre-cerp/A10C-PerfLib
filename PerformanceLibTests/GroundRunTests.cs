using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class GroundRunTests
{

    [Theory]
    [InlineData(4.0, 30000, 4500)]
    [InlineData(5.0, 35000, 5900)]
    [InlineData(7.0, 45000, 8300)]
    [InlineData(7.4, 30000, 2750)] 
    [InlineData(11.0, 30000, 800)]
    [InlineData(4.0, 32500, 5650)] 
    [InlineData(4.5, 30000, 4280)] 
    [InlineData(4.5, 32500, 5320)] 
    public void TakeoffGroundRunFlapsTO_ReturnsExpected(double takeoffIndex, double grossWeight, double expected)
    {
        double result = TakeoffGroundRun(takeoffIndex, grossWeight , 0,FLAPS.TO);
        Assert.InRange(result, expected - 10, expected + 10);
    }

    [Theory]
    [InlineData(4.0, 30000, 4800)]
    [InlineData(5.0, 35000, 6430)]
    [InlineData(7.0, 45000, 9050)]
    [InlineData(7.4, 30000, 2936)]
    [InlineData(11.0, 30000, 970)]
    [InlineData(4.0, 32500, 6100)]
    [InlineData(4.5, 30000, 4530)]
    [InlineData(4.5, 32500, 5720)]
    public void TakeoffGroundRunFlapsUP_ReturnsExpected(double takeoffIndex, double grossWeight, double expected)
    {
        double result = TakeoffGroundRun(takeoffIndex, grossWeight, 0, FLAPS.UP);
        Assert.InRange(result, expected - 10, expected + 10);
    }


    [Theory]
    [InlineData(3.9, 30000)]
    [InlineData(11.1, 30000)]
    public void TakeoffGroundRun_TakeoffIndexOutOfRange_Throws(double takeoffIndex, double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TakeoffGroundRun(takeoffIndex, grossWeight , 0, FLAPS.TO));
    }

    [Theory]
    [InlineData(EMPTY_WEIGHT_LBS - 1, 6.0)]
    [InlineData(MAX_TAKEOFF_WEIGHT_LBS + 1, 6.0)]
    public void TakeoffGroundRun_GrossWeightOutOfRange_Throws(double grossWeight, double takeoffIndex)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TakeoffGroundRun(takeoffIndex, grossWeight, 0, FLAPS.TO));
    }

    [Theory]
    [InlineData(4.0, 50000)] 
    [InlineData(5.0, 50000)] 
    public void TakeoffGroundRun_TooHeavyForIndex_Throws(double takeoffIndex, double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TakeoffGroundRun(takeoffIndex, grossWeight , 0,FLAPS.TO));
    }

    [Fact]
    public void TakeoffGroundRun_WindCorrection_Works()
    {
        // No wind
        double resultNoWind = TakeoffGroundRun(7.0, 45000, 0,FLAPS.TO);
        Assert.InRange(resultNoWind, 8300 - 10, 8300 + 10);
        // 10 knots headwind should reduce distance
        double resultHeadwind = TakeoffGroundRun(7.0, 45000, 10, FLAPS.TO);
        Assert.True(resultHeadwind < resultNoWind);
        // 10 knots tailwind should increase distance
        double resultTailwind = TakeoffGroundRun(7.0, 45000, -10, FLAPS.TO);
        Assert.True(resultTailwind > resultNoWind);
        // Extreme headwind should reduce distance further
        double resultStrongHeadwind = TakeoffGroundRun(7.0, 45000, 30, FLAPS.TO);
        Assert.True(resultStrongHeadwind < resultHeadwind);
        // Extreme tailwind should increase distance further
        double resultStrongTailwind = TakeoffGroundRun(7.0, 45000, -20, FLAPS.TO);
        Assert.True(resultStrongTailwind > resultTailwind);
    }
}