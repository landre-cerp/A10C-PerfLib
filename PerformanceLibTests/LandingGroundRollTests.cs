using a10c_perf_lib.src;
using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class LandingGroundRollTests
{
    // Spot checks verified against TS polynomial reference (no wind, DRY, tolerance ±10 ft)
    [Theory]
    [InlineData(100, 30000, 1175)] // LI=100, 30k lbs
    [InlineData( 90, 35000, 1333)] // LI=90,  35k lbs
    public void LandingGroundRoll_SBOpen_NoWind_DryCalibration(double li, double weight, double expected)
    {
        double result = LandingGroundRoll(li, weight, Speedbrakes.Open, 0, RCR.DRY);
        Assert.InRange(result, expected - 10, expected + 10);
    }

    // Spot check with headwind — verified against TS (LI=90, 35k, 10kt headwind, DRY ≈ 1112 ft)
    [Fact]
    public void LandingGroundRoll_SBOpen_HeadwindSpotCheck()
    {
        double result = LandingGroundRoll(90, 35000, Speedbrakes.Open, 10, RCR.DRY);
        Assert.InRange(result, 1102, 1122);
    }

    [Fact]
    public void LandingGroundRoll_SBOpen_HeadwindReducesDistance()
    {
        double noWind  = LandingGroundRoll(95, 35000, Speedbrakes.Open,   0, RCR.DRY);
        double headwind = LandingGroundRoll(95, 35000, Speedbrakes.Open,  15, RCR.DRY);
        double tailwind = LandingGroundRoll(95, 35000, Speedbrakes.Open, -10, RCR.DRY);

        Assert.True(headwind < noWind);
        Assert.True(tailwind > noWind);
    }

    [Fact]
    public void LandingGroundRoll_SBClosed_LongerThanSBOpen()
    {
        double sbOpen   = LandingGroundRoll(95, 35000, Speedbrakes.Open,   0, RCR.DRY);
        double sbClosed = LandingGroundRoll(95, 35000, Speedbrakes.Closed, 0, RCR.DRY);

        Assert.True(sbClosed > sbOpen);
    }

    [Fact]
    public void LandingGroundRoll_IcyLongerThanDry()
    {
        double dry = LandingGroundRoll(95, 35000, Speedbrakes.Open, 0, RCR.DRY);
        double wet = LandingGroundRoll(95, 35000, Speedbrakes.Open, 0, RCR.WET);
        double icy = LandingGroundRoll(95, 35000, Speedbrakes.Open, 0, RCR.ICY);

        Assert.True(dry < wet);
        Assert.True(wet < icy);
    }

    [Fact]
    public void LandingGroundRoll_MinSpeed_SBOpen_Reduces180Ft()
    {
        double normal  = LandingGroundRoll(95, 35000, Speedbrakes.Open, 0, RCR.DRY, minSpeed: false);
        double minSpd  = LandingGroundRoll(95, 35000, Speedbrakes.Open, 0, RCR.DRY, minSpeed: true);

        Assert.InRange(normal - minSpd, 175, 185);
    }

    [Fact]
    public void LandingGroundRoll_MinSpeed_SBClosed_Reduces250Ft()
    {
        double normal  = LandingGroundRoll(95, 35000, Speedbrakes.Closed, 0, RCR.DRY, minSpeed: false);
        double minSpd  = LandingGroundRoll(95, 35000, Speedbrakes.Closed, 0, RCR.DRY, minSpeed: true);

        Assert.InRange(normal - minSpd, 245, 255);
    }

    [Fact]
    public void LandingGroundRoll_HeavierIncreases()
    {
        double light  = LandingGroundRoll(95, 30000, Speedbrakes.Open, 0, RCR.DRY);
        double heavy  = LandingGroundRoll(95, 45000, Speedbrakes.Open, 0, RCR.DRY);

        Assert.True(heavy > light);
    }

    [Theory]
    [InlineData(69.9, 30000)]
    [InlineData(120.1, 30000)]
    public void LandingGroundRoll_IndexOutOfRange_Throws(double li, double weight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            LandingGroundRoll(li, weight, Speedbrakes.Open, 0, RCR.DRY));
    }
}
