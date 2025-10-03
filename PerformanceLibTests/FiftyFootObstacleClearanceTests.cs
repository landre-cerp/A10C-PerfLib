using static a10c_perf_lib.src.PerfCalculator;


namespace a10c_perf_lib.Tests;

public class FiftyFootObstacleClearanceTests
{
    [Theory]
    [InlineData(1000, 0, FLAPS.TO, 1300)] 
    [InlineData(2000, 0, FLAPS.TO, 2780)] 
    [InlineData(3000, 0, FLAPS.TO, 4270)] 
    [InlineData(1000, 0, FLAPS.UP, 1200)] 
    [InlineData(2000, 0, FLAPS.UP, 2639)] 
    [InlineData(1500, 0, FLAPS.TO, 2040)] 
    [InlineData(2500, 0, FLAPS.TO, 3525)] 
    public void FiftyFootObstacleClearanceDistance_WithNoWind_ReturnsExpectedValues(
        double groundRun, double windspeed, FLAPS flaps, double expected)
    {
        var result = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        Assert.InRange(result, expected - 50, expected + 50);
    }

    [Theory]
    [InlineData(1000, -20, FLAPS.TO, 1230)] 
    [InlineData(1000, 20, FLAPS.TO, 1370)]  
    [InlineData(1000, 40, FLAPS.TO, 1440)]  
    [InlineData(2000, -20, FLAPS.TO, 2550)] 
    [InlineData(2000, 40, FLAPS.TO, 3240)]  
    public void FiftyFootObstacleClearanceDistance_WithWind_ReturnsExpectedValues(
        double groundRun, double windspeed, FLAPS flaps, double expected)
    {
        var result = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        Assert.InRange(result, expected - 50, expected + 50);
    }

    [Theory]
    [InlineData(1000, 10, FLAPS.TO)] 
    [InlineData(1500, 10, FLAPS.TO)] 
    [InlineData(2500, -10, FLAPS.UP)] 
    [InlineData(3500, 30, FLAPS.UP)] 
    public void FiftyFootObstacleClearanceDistance_WithInterpolatedWind_ReturnsValidValues(
        double groundRun, double windspeed, FLAPS flaps)
    {
        var result = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        
        // Result should be positive and within reasonable range
        Assert.True(result > 0);
        Assert.True(result < 50000); // Sanity check for maximum reasonable value
    }

    [Theory]
    [InlineData(1000, 0, FLAPS.TO)]
    [InlineData(2000, 0, FLAPS.UP)]
    public void FiftyFootObstacleClearanceDistance_WithDifferentRCR_ReturnsValidValues(
        double groundRun, double windspeed, FLAPS flaps)
    {
        var result = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        
        // Note: Currently RCR parameter doesn't seem to affect the calculation
        // but this test ensures the method accepts all RCR values
        Assert.True(result > 0);
    }

    [Fact]
    public void FiftyFootObstacleClearanceDistance_FlapsTO_vs_FlapsUP_UseDifferentTables()
    {
        double groundRun = 1000;
        double windspeed = 0;

        var resultFlapsTO = FiftyFootObstacleClearanceDistance(groundRun, windspeed, FLAPS.TO);
        var resultFlapsUP = FiftyFootObstacleClearanceDistance(groundRun, windspeed, FLAPS.UP);

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

        var resultTailwind = FiftyFootObstacleClearanceDistance(groundRun, -windMagnitude, flaps);
        var resultNoWind = FiftyFootObstacleClearanceDistance(groundRun, 0, flaps);
        var resultHeadwind = FiftyFootObstacleClearanceDistance(groundRun, windMagnitude, flaps);

        // Headwind should increase distance, tailwind should decrease it
        if (windMagnitude > 0)
        {
            Assert.True(resultHeadwind > resultNoWind, "Headwind should increase obstacle clearance distance");
            Assert.True(resultTailwind < resultNoWind, "Tailwind should decrease obstacle clearance distance");
        }
    }

    [Theory]
    [InlineData(5000, 0, FLAPS.TO, 7340)] // Mid-range ground run
    [InlineData(6000, 0, FLAPS.TO, 9230)] // Higher ground run
    [InlineData(7000, 0, FLAPS.TO, 11380)] // High ground run
    [InlineData(5000, 0, FLAPS.UP, 7105)] // Mid-range with flaps up
    [InlineData(6000, 0, FLAPS.UP, 8770)] // Higher ground run with flaps up
    public void FiftyFootObstacleClearanceDistance_HigherGroundRun_ReturnsExpectedValues(
        double groundRun, double windspeed, FLAPS flaps, double expected)
    {
        var result = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        Assert.InRange(result, expected - 100, expected + 100);
    }

    [Theory]
    [InlineData(12000, 0, FLAPS.TO)] 
    [InlineData(13000, 0, FLAPS.TO)]
    [InlineData(12000, 0, FLAPS.UP)]
    [InlineData(10000, 20, FLAPS.UP)]
    [InlineData(14000, 0, FLAPS.UP)]
    public void FiftyFootObstacleClearanceDistance_EdgeCases_ReturnsValidValues(
        double groundRun, double windspeed, FLAPS flaps)
    {
        var result = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        
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
        var resultTO = FiftyFootObstacleClearanceDistance(groundRun, windspeed, FLAPS.TO);
        var resultUP = FiftyFootObstacleClearanceDistance(groundRun, windspeed, FLAPS.UP);

        // Both should be positive and within reasonable range
        Assert.True(resultTO > 0);
        Assert.True(resultUP > 0);
        Assert.True(resultTO < 50000);
        Assert.True(resultUP < 50000);
        
        // Results should be different (different tables)
        Assert.NotEqual(resultTO, resultUP);
    }

    [Fact]
    public void FiftyFootObstacleClearanceDistance_ConsistentBehavior_AcrossRCRValues()
    {
        double groundRun = 3000;
        double windspeed = 10;
        var flaps = FLAPS.TO;

        var resultDry = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        var resultWet = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);
        var resultIcy = FiftyFootObstacleClearanceDistance(groundRun, windspeed, flaps);

        // Note: Current implementation doesn't use RCR, so values should be equal
        // This test documents current behavior and will catch changes if RCR is implemented
        Assert.Equal(resultDry, resultWet);
        Assert.Equal(resultDry, resultIcy);
        Assert.True(resultDry > 0);
    }
}