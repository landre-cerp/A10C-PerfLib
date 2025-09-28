using a10c_perf_lib.src;

namespace a10c_perf_lib.Tests;

public class A10CPerfCalculatorTests
{
    private readonly A10CPerfCalculator _calc = new();

    [Theory]
    [InlineData(-40, 85.1)]
    [InlineData(-35, 85.8)]
    [InlineData(-20, 86.1)]
    [InlineData(0, 84.5)]
    [InlineData(20, 82.1)]
    [InlineData(50, 77.6)]
    public void PTFS_ReturnsExpected(double temp, double expected)
    {
        double result = _calc.PTFS(temp);
        Assert.InRange(result, expected - 0.5, expected + 0.5);
    }

    [Theory]
    [InlineData(-41)]
    [InlineData(51)]
    public void PTFS_OutOfRange_Throws(double temp)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calc.PTFS(temp));
    }

    [Theory]
    [InlineData(A10CPerfCalculator.EMPTY_WEIGHT_LBS, 109.69)]
    [InlineData(30000, 118.60)]
    [InlineData(40000, 137.03)]
    [InlineData(A10CPerfCalculator.MAX_TAKEOFF_WEIGHT_LBS, 147.77)]
    public void TakeOffSpeed_ReturnsExpected(double grossWeight, double expected)
    {
        double result = _calc.TakeOffSpeed(grossWeight);
        Assert.Equal(expected, result, 2); 
    }

    [Theory]
    [InlineData(A10CPerfCalculator.EMPTY_WEIGHT_LBS - 1)]
    [InlineData(A10CPerfCalculator.MAX_TAKEOFF_WEIGHT_LBS + 1 )]
    public void TakeOffSpeed_OutOfRange_Throws(double grossWeight)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calc.TakeOffSpeed(grossWeight));
    }

    [Theory]
    [InlineData(-30, 0, 10.8)]
    [InlineData(0, 2_000, 9.82)]
    [InlineData(10, 2_000, 9.6)]
    [InlineData(20, 4_000, 8.6)]
    [InlineData(50, 6_000, 5.0)]
    public void GetTakeoffIndex_MaxThrust_ReturnsExpected(double tempC, double altFt, double expected)
    {
        double result = A10CPerfCalculator.TakeoffIndex(
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
        double result = A10CPerfCalculator.TakeoffIndex(
            tempC,
            new PressureAltitude(altFt, QNH.StdInHg), false);

        Assert.Equal(expected, result, 1);
    }


    [Theory]
    [InlineData(-31)]
    [InlineData(51)]
    public void GetTakeoffIndex_TemperatureOutOfRange_Throws(double tempC)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => A10CPerfCalculator.TakeoffIndex(tempC, new PressureAltitude(0, QNH.StdInHg)));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(6001)]
    public void GetTakeoffIndex_AltitudeOutOfRange_Throws(double altFt)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => A10CPerfCalculator.TakeoffIndex(
            0,
            new PressureAltitude(altFt, QNH.StdInHg)));
    }
}