using a10c_perf_lib.src;

namespace a10c_perf_lib.Tests;

public class TakeoffIndexTests
{
    private readonly PerfCalculator _calc = new();

    [Theory]
    [InlineData(-30, 0, 10.8)]
    [InlineData(0, 2_000, 9.82)]
    [InlineData(10, 2_000, 9.6)]
    [InlineData(20, 4_000, 8.6)]
    [InlineData(50, 6_000, 5.0)]
    public void GetTakeoffIndex_MaxThrust_ReturnsExpected(double tempC, double altFt, double expected)
    {
        double result = PerfCalculator.TakeoffIndex(
            tempC, 
            new PressureAltitude(altFt, QNH.StdInHg));

        Assert.Equal(expected, result, 1); 
    }

    [Theory]
    [InlineData(-30, 0, 10.71)]
    [InlineData(0, 2_000, 9.40)]
    [InlineData(10, 2_000, 9.00)]
    [InlineData(20, 4_000, 7.60)]
    [InlineData(50, 5_000, 4.40)]
    public void GetTakeoffIndex_Three_Percent_Below_ReturnsExpected(double tempC, double altFt, double expected)
    {
        double result = PerfCalculator.TakeoffIndex(
            tempC,
            new PressureAltitude(altFt, QNH.StdInHg), false);

        Assert.Equal(expected, result, 1);
    }


    [Theory]
    [InlineData(-31)]
    [InlineData(51)]
    public void GetTakeoffIndex_TemperatureOutOfRange_Throws(double tempC)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PerfCalculator.TakeoffIndex(tempC, new PressureAltitude(0, QNH.StdInHg)));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6001)]
    public void GetTakeoffIndex_AltitudeOutOfRange_Throws(double altFt)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => PerfCalculator.TakeoffIndex(
            0,
            new PressureAltitude(altFt, QNH.StdInHg)));
    }
}