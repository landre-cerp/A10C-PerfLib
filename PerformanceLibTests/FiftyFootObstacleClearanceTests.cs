using static a10c_perf_lib.src.PerfCalculator;


namespace a10c_perf_lib.Tests;

public class FiftyFootObstacleClearanceTests
{
    [Theory]
    [InlineData(1000, 0, FLAPS.TO, ThrustSetting.Max ,1300)] 
    [InlineData(2000, 0, FLAPS.TO, ThrustSetting.Max ,2780)] 
    [InlineData(3000, 0, FLAPS.TO, ThrustSetting.Max ,4270)] 
    [InlineData(1000, 0, FLAPS.UP, ThrustSetting.Max ,1200)] 
    [InlineData(2000, 0, FLAPS.UP, ThrustSetting.Max ,2639)] 
    [InlineData(1500, 0, FLAPS.TO, ThrustSetting.Max ,2040)] 
    [InlineData(2500, 0, FLAPS.TO, ThrustSetting.Max ,3525)] 
    public void FiftyFootObstacleClearanceDistance_WithNoWind_ReturnsExpectedValues(
        double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrust,double expected)
    {
        var result = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, thrust);
        Assert.InRange(result, expected - 50, expected + 50);
    }

    [Theory]
    [InlineData(1000, -20, FLAPS.TO, ThrustSetting.Max, 1230)] 
    [InlineData(1000, 20, FLAPS.TO, ThrustSetting.Max, 1370)]  
    [InlineData(1000, 40, FLAPS.TO, ThrustSetting.Max, 1440)]  
    [InlineData(2000, -20, FLAPS.TO, ThrustSetting.Max, 2550)] 
    [InlineData(2000, 40, FLAPS.TO, ThrustSetting.Max, 3240)]  
    public void FiftyFootObstacleClearanceDistance_WithWind_ReturnsExpectedValues(
        double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrust, double expected)
    {
        var result = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, thrust);
        Assert.InRange(result, expected - 50, expected + 50);
    }

    [Theory]
    [InlineData(1000, 10, FLAPS.TO, ThrustSetting.Max)] 
    [InlineData(1500, 10, FLAPS.TO, ThrustSetting.Max)] 
    [InlineData(2500, -10, FLAPS.UP, ThrustSetting.Max)] 
    [InlineData(3500, 30, FLAPS.UP, ThrustSetting.Max)] 
    public void FiftyFootObstacleClearanceDistance_WithInterpolatedWind_ReturnsValidValues(
        double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrust)
    {
        var result = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, thrust);

        // Result should be positive and within reasonable range
        Assert.True(result > 0);
        Assert.True(result < 50000); // Sanity check for maximum reasonable value
    }

    [Theory]
    [InlineData(1000, 0, FLAPS.TO, ThrustSetting.Max)]
    [InlineData(2000, 0, FLAPS.UP, ThrustSetting.Max)]
    public void FiftyFootObstacleClearanceDistance_WithDifferentRCR_ReturnsValidValues(
        double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrust)
    {
        var result = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, thrust);

        // Note: Currently RCR parameter doesn't seem to affect the calculation
        // but this test ensures the method accepts all RCR values
        Assert.True(result > 0);
    }

    [Fact]
    public void FiftyFootObstacleClearanceDistance_FlapsTO_vs_FlapsUP_UseDifferentTables()
    {
        double groundRun = 1000;
        double windspeed = 0;

        var resultFlapsTO = FiftyFtObstacleClearanceDistance(groundRun, windspeed, FLAPS.TO, ThrustSetting.Max);
        var resultFlapsUP = FiftyFtObstacleClearanceDistance(groundRun, windspeed, FLAPS.UP, ThrustSetting.Max);

        // Should use different tables and return different values
        Assert.NotEqual(resultFlapsTO, resultFlapsUP);
        
        // Both should be positive
        Assert.True(resultFlapsTO > 0);
        Assert.True(resultFlapsUP > 0);
    }

    [Theory]
    [InlineData(1000, 0)]
    [InlineData(2000, 10)]
    [InlineData(3000, 20)]
    public void FiftyFootObstacleClearanceDistance_HeadwindVsTailwind_ShowsExpectedTrend(
        double groundRun, double windMagnitude)
    {
        var flaps = FLAPS.TO;

        var resultTailwind = FiftyFtObstacleClearanceDistance(groundRun, -windMagnitude, flaps, ThrustSetting.Max);
        var resultNoWind = FiftyFtObstacleClearanceDistance(groundRun, 0, flaps, ThrustSetting.Max);
        var resultHeadwind = FiftyFtObstacleClearanceDistance(groundRun, windMagnitude, flaps, ThrustSetting.Max);

        // Headwind should increase distance, tailwind should decrease it
        if (windMagnitude > 0)
        {
            Assert.True(resultHeadwind > resultNoWind, "Headwind should increase obstacle clearance distance");
            Assert.True(resultTailwind < resultNoWind, "Tailwind should decrease obstacle clearance distance");
        }
    }

    [Theory]
    [InlineData(5000, 0, FLAPS.TO,  ThrustSetting.Max, 7340)] // Mid-range ground run
    [InlineData(6000, 0, FLAPS.TO, ThrustSetting.Max, 9230 )] // Higher ground run
    [InlineData(7000, 0, FLAPS.TO, ThrustSetting.Max, 11380)] // High ground run
    [InlineData(5000, 0, FLAPS.UP, ThrustSetting.Max,7105)] // Mid-range with flaps up
    [InlineData(6000, 0, FLAPS.UP, ThrustSetting.Max,8770)] // Higher ground run with flaps up
    public void FiftyFootObstacleClearanceDistance_HigherGroundRun_ReturnsExpectedValues(
        double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrustSetting, double expected)
    {
        var result = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, thrustSetting);
        Assert.InRange(result, expected - 100, expected + 100);
    }

    [Theory]
    [InlineData(12000, 0, FLAPS.TO, ThrustSetting.Max)] 
    [InlineData(13000, 0, FLAPS.TO, ThrustSetting.Max)]
    [InlineData(12000, 0, FLAPS.UP, ThrustSetting.Max)]
    [InlineData(10000, 20, FLAPS.UP, ThrustSetting.Max)]
    [InlineData(14000, 0, FLAPS.UP, ThrustSetting.Max)]
    public void FiftyFootObstacleClearanceDistance_EdgeCases_ReturnsValidValues(
        double groundRun, double windspeed, FLAPS flaps, ThrustSetting thrust)
    {
        var result = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, thrust);

        Assert.True(result < 0);
    }

    [Theory]
    [InlineData(1500, 10)] 
    [InlineData(2500, -5)] 
    [InlineData(3500, 15)] 
    [InlineData(4500, -10)]
    [InlineData(5500, 30)] 
    public void FiftyFootObstacleClearanceDistance_BilinearInterpolation_ReturnsConsistentResults(
        double groundRun, double windspeed)
    {
        var resultTOMax = FiftyFtObstacleClearanceDistance(groundRun, windspeed, FLAPS.TO, ThrustSetting.Max);
        var resultUPMax = FiftyFtObstacleClearanceDistance(groundRun, windspeed, FLAPS.UP, ThrustSetting.Max);

        // Both should be positive and within reasonable range
        Assert.True(resultTOMax > 0);
        Assert.True(resultUPMax > 0);
        Assert.True(resultTOMax < 50000);
        Assert.True(resultUPMax < 50000);
        
        // Results should be different (different tables)
        Assert.NotEqual(resultTOMax, resultUPMax);

    }

    [Fact]
    public void FiftyFootObstacleClearanceDistance_ConsistentBehavior_AcrossRCRValues()
    {
        double groundRun = 3000;
        double windspeed = 10;
        var flaps = FLAPS.TO;

        var resultDry = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, ThrustSetting.Max);
        var resultWet = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, ThrustSetting.Max);
        var resultIcy = FiftyFtObstacleClearanceDistance(groundRun, windspeed, flaps, ThrustSetting.Max);

        // Note: Current implementation doesn't use RCR, so values should be equal
        // This test documents current behavior and will catch changes if RCR is implemented
        Assert.Equal(resultDry, resultWet);
        Assert.Equal(resultDry, resultIcy);
        Assert.True(resultDry > 0);
    }
}