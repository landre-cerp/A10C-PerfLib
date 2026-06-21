using a10c_perf_lib.src;
using static a10c_perf_lib.src.PerfCalculator;

namespace a10c_perf_lib.Tests;

public class TakeoffTests
{
    // ── Ground run spot checks ──────────────────────────────────────────────
    // Inputs must be valid for both GroundRun and CriticalField tables.

    [Theory]
    [InlineData(4.0, 30000, 0, FLAPS.TO, 4500)]
    [InlineData(5.0, 35000, 0, FLAPS.TO, 5900)]
    [InlineData(7.4, 30000, 0, FLAPS.TO, 2750)]
    [InlineData(11.0, 30000, 0, FLAPS.TO, 800)]
    [InlineData(4.0, 30000, 0, FLAPS.UP, 4800)]
    [InlineData(5.0, 35000, 0, FLAPS.UP, 6430)]
    public void GroundRunFt_KnownInputs_MatchReference(double index, double gw, double wind, FLAPS flaps, double expected)
    {
        double result = Takeoff(index, gw, wind, flaps, ThrustSetting.Max, RCR.DRY).GroundRunFt;
        Assert.InRange(result, expected - 10, expected + 10);
    }

    [Fact]
    public void GroundRunFt_FlapsTO_vs_FlapsUP_Differs()
    {
        double grTo = Takeoff(8.0, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).GroundRunFt;
        double grUp = Takeoff(8.0, 35000, 0, FLAPS.UP, ThrustSetting.Max, RCR.DRY).GroundRunFt;
        Assert.NotEqual(grTo, grUp);
    }

    [Fact]
    public void GroundRunFt_HeadwindReduces_TailwindIncreases()
    {
        double noWind   = Takeoff(8.0, 35000,   0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).GroundRunFt;
        double headwind = Takeoff(8.0, 35000,  10, FLAPS.TO, ThrustSetting.Max, RCR.DRY).GroundRunFt;
        double tailwind = Takeoff(8.0, 35000, -10, FLAPS.TO, ThrustSetting.Max, RCR.DRY).GroundRunFt;
        Assert.True(headwind < noWind,   $"Headwind={headwind} should be less than no-wind={noWind}");
        Assert.True(tailwind > noWind,   $"Tailwind={tailwind} should exceed no-wind={noWind}");
    }

    // ── Critical field length ───────────────────────────────────────────────

    [Theory]
    [InlineData(7.0, 30000, 0, FLAPS.TO, RCR.DRY, 4500)]
    [InlineData(8.0, 35000, 0, FLAPS.TO, RCR.DRY, 5270)]
    [InlineData(9.0, 40000, 0, FLAPS.TO, RCR.DRY, 5700)]
    [InlineData(10.0, 45000, 0, FLAPS.TO, RCR.DRY, 5340)]
    public void CriticalFieldFt_KnownInputs_MatchReference(double index, double gw, double wind, FLAPS flaps, RCR rcr, double expected)
    {
        double result = Takeoff(index, gw, wind, flaps, ThrustSetting.Max, rcr).CriticalFieldFt;
        Assert.InRange(result, expected - 50, expected + 50);
    }

    [Fact]
    public void CriticalFieldFt_HeadwindReduces_TailwindIncreases()
    {
        double noWind   = Takeoff(8.0, 35000,   0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).CriticalFieldFt;
        double headwind = Takeoff(8.0, 35000,  10, FLAPS.TO, ThrustSetting.Max, RCR.DRY).CriticalFieldFt;
        double tailwind = Takeoff(8.0, 35000, -10, FLAPS.TO, ThrustSetting.Max, RCR.DRY).CriticalFieldFt;
        Assert.True(headwind < noWind,  $"Headwind={headwind} should be less than no-wind={noWind}");
        Assert.True(tailwind > noWind,  $"Tailwind={tailwind} should exceed no-wind={noWind}");
    }

    [Fact]
    public void CriticalFieldFt_FlapsUP_SevenPercentMoreThanTO()
    {
        double cflTo = Takeoff(8.0, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).CriticalFieldFt;
        double cflUp = Takeoff(8.0, 35000, 0, FLAPS.UP, ThrustSetting.Max, RCR.DRY).CriticalFieldFt;
        Assert.InRange(cflUp, cflTo * 1.07 - 10, cflTo * 1.07 + 10);
    }

    [Fact]
    public void CriticalFieldFt_WetRunway_LongerThanDry()
    {
        double dry = Takeoff(8.0, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).CriticalFieldFt;
        double wet = Takeoff(8.0, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.WET).CriticalFieldFt;
        Assert.True(wet > dry, $"Wet={wet} should exceed dry={dry}");
    }

    // ── 50 ft obstacle clearance ────────────────────────────────────────────

    [Fact]
    public void FiftyFtClearanceFt_IsPositive_ForNormalInputs()
    {
        double result = Takeoff(8.0, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        Assert.True(result > 0, $"FiftyFtClearance={result} should be positive");
    }

    [Fact]
    public void FiftyFtClearanceFt_HeadwindReduces_TailwindIncreases()
    {
        // Headwind shortens the ground run, which in turn reduces the 50ft clearance distance
        double noWind   = Takeoff(8.0, 35000,   0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        double headwind = Takeoff(8.0, 35000,  20, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        double tailwind = Takeoff(8.0, 35000, -20, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        Assert.True(headwind < noWind,  $"Headwind={headwind} should be less than no-wind={noWind}");
        Assert.True(tailwind > noWind,  $"Tailwind={tailwind} should exceed no-wind={noWind}");
    }

    [Fact]
    public void FiftyFtClearanceFt_FlapsTO_vs_FlapsUP_Differs()
    {
        double to = Takeoff(8.0, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        double up = Takeoff(8.0, 35000, 0, FLAPS.UP, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        Assert.NotEqual(to, up);
    }

    [Fact]
    public void FiftyFtClearanceFt_HeavierAircraft_MoreDistance()
    {
        // Heavier aircraft has a longer ground run, which drives a longer 50ft clearance
        double light = Takeoff(9.0, 30000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        double heavy = Takeoff(9.0, 40000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).FiftyFtClearanceFt;
        Assert.True(heavy > light, $"Heavy={heavy} should exceed light={light}");
    }

    // ── Takeoff and rotation speed ──────────────────────────────────────────

    [Theory]
    [InlineData(30000, FLAPS.TO, 118.60)]
    [InlineData(40000, FLAPS.TO, 137.03)]
    public void TakeoffSpeedKts_KnownValues_MatchReference(double gw, FLAPS flaps, double expected)
    {
        double result = Takeoff(8.0, gw, 0, flaps, ThrustSetting.Max, RCR.DRY).TakeoffSpeedKts;
        Assert.Equal(expected, result, 2);
    }

    [Fact]
    public void TakeoffSpeedKts_FlapsUP_FasterThanFlapsTO()
    {
        double speedTo = Takeoff(8.0, 30000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY).TakeoffSpeedKts;
        double speedUp = Takeoff(8.0, 30000, 0, FLAPS.UP, ThrustSetting.Max, RCR.DRY).TakeoffSpeedKts;
        Assert.InRange(speedUp - speedTo, 4, 5);
    }

    [Fact]
    public void RotationSpeedKts_IsTenLessThanTakeoffSpeed()
    {
        var result = Takeoff(8.0, 30000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY);
        Assert.Equal(result.TakeoffSpeedKts - 10, result.RotationSpeedKts, 10);
    }

    // ── Argument validation ─────────────────────────────────────────────────

    [Theory]
    [InlineData(3.9)]
    [InlineData(11.1)]
    public void Takeoff_InvalidTakeoffIndex_Throws(double index)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Takeoff(index, 35000, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY));
    }

    [Theory]
    [InlineData(EMPTY_WEIGHT_LBS - 1)]
    [InlineData(MAX_TAKEOFF_WEIGHT_LBS + 1)]
    public void Takeoff_InvalidGrossWeight_Throws(double gw)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Takeoff(7.0, gw, 0, FLAPS.TO, ThrustSetting.Max, RCR.DRY));
    }
}
