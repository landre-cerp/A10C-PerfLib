using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class ClimbTests
{
    // ── Direction sanity tests ──────────────────────────────────────────────

    [Fact]
    public void ClimbDistance_HeavierWeight_RequiresMoreDistance()
    {
        double light = ClimbDistance(25000, 40000, 30000, 0);
        double heavy = ClimbDistance(25000, 40000, 45000, 0);
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    [Fact]
    public void ClimbDistance_MoreDrag_RequiresMoreDistance()
    {
        double clean = ClimbDistance(25000, 40000, 35000, 0);
        double dirty = ClimbDistance(25000, 40000, 35000, 8);
        Assert.True(dirty > clean, $"Drag8={dirty} should exceed clean={clean}");
    }

    [Fact]
    public void ClimbFuel_HeavierWeight_UseMoreFuel()
    {
        double light = ClimbFuel(25000, 40000, 30000, 0);
        double heavy = ClimbFuel(25000, 40000, 45000, 0);
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    [Fact]
    public void ClimbTime_LongerSegment_TakesMoreTime()
    {
        double short_ = ClimbTime(35000, 40000, 35000, 0);
        double long_  = ClimbTime(25000, 45000, 35000, 0);
        Assert.True(long_ > short_, $"Long={long_} should exceed short={short_}");
    }

    [Fact]
    public void ClimbDistance_HotDay_IncreasesDistance()
    {
        // Short segment so base delta stays within the temp correction table's X range (≤125 NM)
        double isa    = ClimbDistance(35000, 37500, 35000, 4, 0);
        double hotDay = ClimbDistance(35000, 37500, 35000, 4, 30);
        Assert.True(hotDay > isa, $"Hot day={hotDay} should exceed ISA={isa}");
    }

    [Fact]
    public void ClimbFuel_HotDay_IncreasesFuel()
    {
        double isa    = ClimbFuel(35000, 37500, 35000, 4, 0);
        double hotDay = ClimbFuel(35000, 37500, 35000, 4, 30);
        Assert.True(hotDay > isa, $"Hot day={hotDay} should exceed ISA={isa}");
    }

    // ── Boundary conditions ─────────────────────────────────────────────────

    [Fact]
    public void ClimbDistance_StartBelowTableRange_TreatedAsZeroCumulative()
    {
        // Start below FL250 → cumulative at start = 0 (pre-range)
        // Should equal starting from FL250 with a target above FL250
        double fromBelow  = ClimbDistance(10000, 35000, 35000, 0);
        double fromBottom = ClimbDistance(25000, 35000, 35000, 0);
        // Both start at 0 cumulative, so should be equal
        Assert.Equal(fromBelow, fromBottom, 1.0);
    }

    [Fact]
    public void ClimbDistance_ZeroSegment_ReturnsZero()
    {
        double result = ClimbDistance(35000, 35000, 35000, 0);
        Assert.Equal(0, result);
    }

    // ── Argument validation ─────────────────────────────────────────────────

    [Fact]
    public void ClimbDistance_TargetAboveMax_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ClimbDistance(25000, 51000, 35000, 0));
    }

    [Fact]
    public void ClimbDistance_StartAboveTarget_Throws()
    {
        Assert.Throws<ArgumentException>(() => ClimbDistance(40000, 35000, 35000, 0));
    }

    [Fact]
    public void ClimbDistance_InvalidDrag_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => ClimbDistance(25000, 40000, 35000, 9));
    }

    // ── Intermediate drag interpolation ────────────────────────────────────

    [Fact]
    public void ClimbDistance_IntermediateDrag_BetweenBoundaries()
    {
        double drag0 = ClimbDistance(25000, 40000, 35000, 0);
        double drag4 = ClimbDistance(25000, 40000, 35000, 4);
        double drag2 = ClimbDistance(25000, 40000, 35000, 2);
        // drag2 interpolates between drag0 and drag4
        Assert.True(drag0 <= drag2 && drag2 <= drag4, $"drag2={drag2} should be between drag0={drag0} and drag4={drag4}");
        // Note: drag4 vs drag8 is not necessarily monotonic — more drag can reduce horizontal
        // distance (lower airspeed = less distance per minute, even though climb takes longer)
    }
}
