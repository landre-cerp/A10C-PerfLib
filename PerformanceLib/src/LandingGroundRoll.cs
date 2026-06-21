using a10c_perf_lib.src.CorrectionTables;

namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    private static readonly LandingGroundRollFlaps20SB100Table LandingGroundRollSB100Table = new();
    private static readonly LandingGroundRollFlaps20SB0Table LandingGroundRollSB0Table = new();
    private static readonly LandingWindCorrectionTable LandingWindCorrection = new();
    private static readonly LandingRcrCorrectionTable LandingRcrCorrection = new();

    /// <summary>
    /// Calculate the landing ground roll in feet (flaps 20°).
    /// </summary>
    /// <param name="landingIndex">Calculated landing index</param>
    /// <param name="grossWeight">Landing gross weight in lbs</param>
    /// <param name="speedbrakes">Speedbrake setting</param>
    /// <param name="windSpeed">Headwind component in knots (negative = tailwind)</param>
    /// <param name="rcr">Runway condition rating</param>
    /// <param name="minSpeed">Whether minimum landing speed technique is used</param>
    public static double LandingGroundRoll(
        LandingIndex landingIndex,
        GrossWeight grossWeight,
        Speedbrakes speedbrakes,
        double windSpeed,
        RCR rcr,
        bool minSpeed = false)
    {
        CorrectionTable groundRollTable = speedbrakes == Speedbrakes.Open
            ? LandingGroundRollSB100Table
            : LandingGroundRollSB0Table;

        double groundRoll = groundRollTable.Interpolate(landingIndex, grossWeight);

        if (windSpeed != 0)
            groundRoll = LandingWindCorrection.Interpolate(groundRoll, windSpeed);

        groundRoll = LandingRcrCorrection.Interpolate(groundRoll, (double)rcr);

        if (minSpeed)
            groundRoll -= speedbrakes == Speedbrakes.Open ? 180 : 250;

        return groundRoll;
    }

    public static double LandingGroundRoll(
        double landingIndex,
        double grossWeight,
        Speedbrakes speedbrakes,
        double windSpeed,
        RCR rcr,
        bool minSpeed = false)
    {
        return LandingGroundRoll(
            new LandingIndex(landingIndex),
            new GrossWeight(grossWeight),
            speedbrakes, windSpeed, rcr, minSpeed);
    }
}
