using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class CriticalFieldLengthTests
{
    [Theory]
    [InlineData(3.5, 35000.0)] // Below minimum takeoff index
    [InlineData(11.5, 35000.0)] // Above maximum takeoff index
    public void CriticalFieldLength_WithInvalidTakeoffIndex_ThrowsArgumentOutOfRangeException(
        double takeoffIndex, double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            CriticalFieldLength(takeoffIndex, grossWeight, 0.0, FLAPS.TO, RCR.DRY));
    }

    [Theory]
    [InlineData(7.0, EMPTY_WEIGHT_LBS-1)] 
    [InlineData(7.0, MAX_TAKEOFF_WEIGHT_LBS+1)] 
    public void CriticalFieldLength_WithInvalidGrossWeight_ThrowsArgumentOutOfRangeException(
        double takeoffIndex, double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            CriticalFieldLength(takeoffIndex, grossWeight, 0.0, FLAPS.TO, RCR.DRY));
    }

    
    [Theory]
    [InlineData(7.0, 30000.0, 0.0, 4500.0)] 
    [InlineData(8.0, 35000.0, 0.0, 5270.0)] 
    [InlineData(9.0, 40000.0, 0.0, 5700.0)] 
    [InlineData(10.0, 45000.0, 0.0, 5340.0)]
    [InlineData(7.5, 32500.0, 0.0, 5000.0)] 
    public void CriticalFieldLength_WithValidInputs_ReturnsExpectedValues(
        double takeoffIndex, double grossWeight, double windspeed, double expected)
    {
        var result = CriticalFieldLength(
            takeoffIndex, grossWeight, windspeed, 
            FLAPS.TO, RCR.DRY);

        Assert.InRange(result, expected - 50, expected + 50); 
    }

    [Fact]
    public void CriticalFieldLength_WindCorrection_Works()
    {

        double resultNoWind = CriticalFieldLength(
            7.0, 35000.0, 0.0, FLAPS.TO, RCR.DRY);

        double resultHeadwind = CriticalFieldLength(
            7.0, 35000.0, 10.0, FLAPS.TO, RCR.DRY);
        Assert.True(resultHeadwind < resultNoWind);

        double resultTailwind = CriticalFieldLength(
            7.0, 35000.0, -10.0, FLAPS.TO, RCR.DRY);
        Assert.True(resultTailwind > resultNoWind);
    }

    [Theory]
    [InlineData(FLAPS.UP, 1.07)] // Flaps UP augmente de 7%
    public void CriticalFieldLength_FlapsCorrection_Works(FLAPS flaps, double expectedMultiplier)
    {
        var baseResult = CriticalFieldLength(
            7.0, 35000.0, 0.0, FLAPS.TO, RCR.DRY);

        var correctedResult = CriticalFieldLength(
            7.0, 35000.0, 0.0, flaps, RCR.DRY);

        Assert.InRange(correctedResult, baseResult * expectedMultiplier - 10, baseResult * expectedMultiplier + 10);
    }
}