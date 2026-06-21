using a10c_perf_lib.src;
using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class LandingSpeedTests
{
    [Theory]
    [InlineData(30000, false, false, 120)] // flaps 20, normal
    [InlineData(46476, false, false, 153)] // max weight
    [InlineData(26000, false, false, 112)] // light weight (above EMPTY_WEIGHT_LBS)
    public void ApproachSpeed_Flaps20_ReturnsExpected(double weight, bool singleEngine, bool minSpeed, double expected)
    {
        double result = PerfCalculator.ApproachSpeed(
            new GrossWeight(weight), LandingFlaps.TWENTY, singleEngine, minSpeed);

        Assert.InRange(result, expected - 1, expected + 1);
    }

    [Theory]
    [InlineData(30000, 130)]
    [InlineData(40000, 150)]
    public void ApproachSpeed_Flaps7_ReturnsExpected(double weight, double expected)
    {
        double result = PerfCalculator.ApproachSpeed(
            new GrossWeight(weight), LandingFlaps.SEVEN);

        Assert.InRange(result, expected - 1, expected + 1);
    }

    [Theory]
    [InlineData(30000, 150)]
    [InlineData(40000, 160)]
    public void ApproachSpeed_SingleEngine_ReturnsExpected(double weight, double expected)
    {
        double result = PerfCalculator.ApproachSpeed(
            new GrossWeight(weight), LandingFlaps.TWENTY, singleEngine: true);

        Assert.InRange(result, expected - 1, expected + 1);
    }

    [Fact]
    public void ApproachSpeed_MinSpeed_LowerThanNormal()
    {
        double normal = PerfCalculator.ApproachSpeed(new GrossWeight(35000), LandingFlaps.TWENTY);
        double min = PerfCalculator.ApproachSpeed(new GrossWeight(35000), LandingFlaps.TWENTY, minSpeed: true);

        Assert.True(min < normal);
    }

    [Fact]
    public void TouchdownSpeed_Normal_IsTenKnotsBelowApproach()
    {
        double approach = PerfCalculator.ApproachSpeed(new GrossWeight(35000), LandingFlaps.TWENTY);
        double touchdown = PerfCalculator.TouchdownSpeed(new GrossWeight(35000), LandingFlaps.TWENTY);

        Assert.Equal(approach - 10, touchdown, 1);
    }

    [Fact]
    public void TouchdownSpeed_MinSpeed_EqualsApproachSpeed()
    {
        var weight = new GrossWeight(35000);
        double approach = PerfCalculator.ApproachSpeed(weight, LandingFlaps.TWENTY, minSpeed: true);
        double touchdown = PerfCalculator.TouchdownSpeed(weight, LandingFlaps.TWENTY, minSpeed: true);

        Assert.Equal(approach, touchdown, 1);
    }
}
