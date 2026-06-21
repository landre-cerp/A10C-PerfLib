using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class DescentTests
{
    // ── Direction sanity tests ──────────────────────────────────────────────

    [Fact]
    public void DescentDistance_StartingHigher_RequiresMoreDistance()
    {
        double short_ = Descent(20000, 10000, 35000, 0).DistanceNm;
        double long_  = Descent(35000, 10000, 35000, 0).DistanceNm;
        Assert.True(long_ > short_, $"Long={long_} should exceed short={short_}");
    }

    [Fact]
    public void DescentFuel_MoreDrag_LessFuel()
    {
        // More drag → steeper glide path → less time at idle → less fuel used
        double clean = Descent(35000, 0, 35000, 0).FuelLbs;
        double dirty = Descent(35000, 0, 35000, 8).FuelLbs;
        Assert.True(dirty < clean, $"Drag8={dirty} should be less than drag0={clean}");
    }

    [Fact]
    public void DescentTime_LongerSegment_TakesMoreTime()
    {
        double short_ = Descent(20000, 10000, 35000, 0).TimeMin;
        double long_  = Descent(35000, 0,     35000, 0).TimeMin;
        Assert.True(long_ > short_, $"Long={long_} should exceed short={short_}");
    }

    [Fact]
    public void DescentFuel_HeavierWeight_UseMoreFuel()
    {
        double light = Descent(35000, 0, 30000, 0).FuelLbs;
        double heavy = Descent(35000, 0, 45000, 0).FuelLbs;
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    // ── Zero-segment boundary ───────────────────────────────────────────────

    [Fact]
    public void DescentDistance_ToSameAltitude_ReturnsZero()
    {
        double result = Descent(25000, 25000, 35000, 0).DistanceNm;
        Assert.Equal(0, result);
    }

    [Fact]
    public void DescentFuel_ToZeroAltitude_PositiveResult()
    {
        double result = Descent(35000, 0, 35000, 0).FuelLbs;
        Assert.True(result > 0, $"Fuel={result} should be positive");
    }

    // ── Max-range speed tests ───────────────────────────────────────────────

    [Fact]
    public void MaxRangeDescentSpeed_HeavierWeight_FasterSpeed()
    {
        double light = MaxRangeDescentSpeed(30000, 0);
        double heavy = MaxRangeDescentSpeed(45000, 0);
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    [Fact]
    public void MaxRangeDescentSpeed_MoreDrag_SlowerSpeed()
    {
        double clean = MaxRangeDescentSpeed(35000, 0);
        double dirty = MaxRangeDescentSpeed(35000, 8);
        Assert.True(dirty < clean, $"Dirty={dirty} should be less than clean={clean}");
    }

    [Theory]
    [InlineData(35000, 0, 159)]
    [InlineData(35000, 8, 151)]
    [InlineData(25000, 0, 135)]
    public void MaxRangeDescentSpeed_KnownValues_MatchTsReference(double weight, double drag, double expected)
    {
        double result = MaxRangeDescentSpeed(weight, drag);
        Assert.Equal(expected, result, 1.0);
    }

    [Fact]
    public void DescentResult_MaxRangeSpeedKts_MatchesStandaloneMethod()
    {
        var result = Descent(35000, 0, 35000, 4);
        double standalone = MaxRangeDescentSpeed(35000, 4);
        Assert.Equal(standalone, result.MaxRangeSpeedKts);
    }

    // ── Argument validation ─────────────────────────────────────────────────

    [Fact]
    public void DescentDistance_StartAboveMax_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Descent(36000, 0, 35000, 0));
    }

    [Fact]
    public void DescentDistance_EndAboveStart_Throws()
    {
        Assert.Throws<ArgumentException>(() => Descent(20000, 25000, 35000, 0));
    }

    [Fact]
    public void DescentDistance_InvalidDrag_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Descent(35000, 0, 35000, 9));
    }

    // ── Approximate spot check vs TS reference ──────────────────────────────

    [Theory]
    [InlineData(35000, 25000, 35000, 0)]  // 10k ft descent, 35k lbs, clean
    [InlineData(35000, 0,     35000, 0)]  // full descent to SL
    public void DescentDistance_ReasonableRange(double startAlt, double endAlt, double weight, double drag)
    {
        double result = Descent(startAlt, endAlt, weight, drag).DistanceNm;
        Assert.True(result > 0 && result < 300, $"Distance {result} NM outside plausible range");
    }

    [Theory]
    [InlineData(35000, 25000, 35000, 0)]
    [InlineData(35000, 0,     35000, 0)]
    public void DescentFuel_ReasonableRange(double startAlt, double endAlt, double weight, double drag)
    {
        double result = Descent(startAlt, endAlt, weight, drag).FuelLbs;
        Assert.True(result > 0 && result < 3000, $"Fuel {result} lbs outside plausible range");
    }

    [Theory]
    [InlineData(35000, 25000, 35000, 0)]
    [InlineData(35000, 0,     35000, 0)]
    public void DescentTime_ReasonableRange(double startAlt, double endAlt, double weight, double drag)
    {
        double result = Descent(startAlt, endAlt, weight, drag).TimeMin;
        Assert.True(result > 0 && result < 60, $"Time {result} min outside plausible range");
    }
}
