using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class ClimbTests
{
    // ── Direction sanity tests ──────────────────────────────────────────────

    [Fact]
    public void ClimbDistance_HeavierWeight_RequiresMoreDistance()
    {
        double light = Climb(25000, 40000, 30000, 0).DistanceNm;
        double heavy = Climb(25000, 40000, 45000, 0).DistanceNm;
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    [Fact]
    public void ClimbDistance_MoreDrag_RequiresMoreDistance()
    {
        double clean = Climb(25000, 40000, 35000, 0).DistanceNm;
        double dirty = Climb(25000, 40000, 35000, 8).DistanceNm;
        Assert.True(dirty > clean, $"Drag8={dirty} should exceed clean={clean}");
    }

    [Fact]
    public void ClimbFuel_HeavierWeight_UseMoreFuel()
    {
        double light = Climb(25000, 40000, 30000, 0).FuelLbs;
        double heavy = Climb(25000, 40000, 45000, 0).FuelLbs;
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    [Fact]
    public void ClimbTime_LongerSegment_TakesMoreTime()
    {
        double short_ = Climb(35000, 40000, 35000, 0).TimeMin;
        double long_  = Climb(25000, 45000, 35000, 0).TimeMin;
        Assert.True(long_ > short_, $"Long={long_} should exceed short={short_}");
    }

    [Fact]
    public void ClimbDistance_HotDay_IncreasesDistance()
    {
        // Short segment so base delta stays within the temp correction table's X range (≤125 NM)
        double isa    = Climb(35000, 37500, 35000, 4, 0).DistanceNm;
        double hotDay = Climb(35000, 37500, 35000, 4, 30).DistanceNm;
        Assert.True(hotDay > isa, $"Hot day={hotDay} should exceed ISA={isa}");
    }

    [Fact]
    public void ClimbFuel_HotDay_IncreasesFuel()
    {
        double isa    = Climb(35000, 37500, 35000, 4, 0).FuelLbs;
        double hotDay = Climb(35000, 37500, 35000, 4, 30).FuelLbs;
        Assert.True(hotDay > isa, $"Hot day={hotDay} should exceed ISA={isa}");
    }

    // ── Boundary conditions ─────────────────────────────────────────────────

    [Fact]
    public void ClimbDistance_StartBelowTableRange_TreatedAsZeroCumulative()
    {
        // Start below FL250 → cumulative at start = 0 (pre-range)
        // Should equal starting from FL250 with a target above FL250
        double fromBelow  = Climb(10000, 35000, 35000, 0).DistanceNm;
        double fromBottom = Climb(25000, 35000, 35000, 0).DistanceNm;
        // Both start at 0 cumulative, so should be equal
        Assert.Equal(fromBelow, fromBottom, 1.0);
    }

    [Fact]
    public void ClimbDistance_ZeroSegment_ReturnsZero()
    {
        double result = Climb(35000, 35000, 35000, 0).DistanceNm;
        Assert.Equal(0, result);
    }

    // ── Argument validation ─────────────────────────────────────────────────

    [Fact]
    public void ClimbDistance_TargetAboveMax_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Climb(25000, 51000, 35000, 0));
    }

    [Fact]
    public void ClimbDistance_StartAboveTarget_Throws()
    {
        Assert.Throws<ArgumentException>(() => Climb(40000, 35000, 35000, 0));
    }

    [Fact]
    public void ClimbDistance_InvalidDrag_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Climb(25000, 40000, 35000, 9));
    }

    // ── Intermediate drag interpolation ────────────────────────────────────

    [Fact]
    public void ClimbDistance_IntermediateDrag_BetweenBoundaries()
    {
        double drag0 = Climb(25000, 40000, 35000, 0).DistanceNm;
        double drag4 = Climb(25000, 40000, 35000, 4).DistanceNm;
        double drag2 = Climb(25000, 40000, 35000, 2).DistanceNm;
        // drag2 interpolates between drag0 and drag4
        Assert.True(drag0 <= drag2 && drag2 <= drag4, $"drag2={drag2} should be between drag0={drag0} and drag4={drag4}");
        // Note: drag4 vs drag8 is not necessarily monotonic — more drag can reduce horizontal
        // distance (lower airspeed = less distance per minute, even though climb takes longer)
    }
}
