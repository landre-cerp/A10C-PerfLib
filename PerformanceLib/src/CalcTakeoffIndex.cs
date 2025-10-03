namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    public static TakeoffIndex CalcTakeoffIndex(double tempC, PressureAltitude altitude, bool isMaxThrust = true)
    {
        CorrectionTable tableToUse = isMaxThrust ? TakeoffIndexMaxThrustTable : TakeoffIndexThreePercentBelowTable;

        return new TakeoffIndex(
            Math.Clamp(
                tableToUse.Interpolate(tempC, altitude.Feet), 
                4.0, 11.0
                )
            );
    }
}
