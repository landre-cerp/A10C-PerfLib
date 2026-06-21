using a10c_perf_lib.src;

namespace a10c_perf_lib.Tests;

public class LandingIndexTests
{
    // Grid-point tests — exact table values, tolerance ±0.1
    [Theory]
    [InlineData(  0,    0, 106.00)]
    [InlineData( 20, 4000,  84.68)]
    [InlineData( 50, 6000,  71.55)]
    [InlineData(-30,    0, 118.82)]
    [InlineData(  0, 2000,  98.20)]
    public void CalcLandingIndex_GridPoint_ReturnsExpected(double tempC, double altFt, double expected)
    {
        LandingIndex result = PerfCalculator.CalcLandingIndex(
            tempC,
            new PressureAltitude(altFt, QNH.StdInHg));

        Assert.InRange(result.Value, expected - 0.1, expected + 0.1);
    }

    // Interpolated values verified against TS polynomial reference (tolerance ±0.5)
    [Theory]
    [InlineData( 5, 1000, 100.3)] // between temp=0/10 and alt=0/2000  (TS=100.26)
    [InlineData(15, 3000,  89.6)] // between temp=10/20 and alt=2000/4000 (TS=89.62)
    public void CalcLandingIndex_Interpolated_MatchesTsReference(double tempC, double altFt, double expected)
    {
        LandingIndex result = PerfCalculator.CalcLandingIndex(
            tempC,
            new PressureAltitude(altFt, QNH.StdInHg));

        Assert.InRange(result.Value, expected - 0.5, expected + 0.5);
    }

    [Fact]
    public void LandingIndex_Hot_High_LowerThan_Cold_Low()
    {
        LandingIndex hotHigh = PerfCalculator.CalcLandingIndex(50, new PressureAltitude(6000, QNH.StdInHg));
        LandingIndex coldLow = PerfCalculator.CalcLandingIndex(-30, new PressureAltitude(0, QNH.StdInHg));

        Assert.True(hotHigh.Value < coldLow.Value);
    }

    [Theory]
    [InlineData(69.9)]
    [InlineData(120.1)]
    public void LandingIndex_OutOfRange_Throws(double value)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new LandingIndex(value));
    }
}
