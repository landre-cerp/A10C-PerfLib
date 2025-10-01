namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    public static TakeoffIndex CalcTakeoffIndex(double tempC, PressureAltitude altitude, bool isMaxThrust = true)
    {

        if (tempC < -30 || tempC > 50)
            throw new ArgumentOutOfRangeException(nameof(tempC), "Temperature out of range (-30 to 50 °C)");

        if (isMaxThrust && (altitude.Feet < 0 || altitude.Feet > 6000))
            throw new ArgumentOutOfRangeException(nameof(altitude), "Altitude out of range (0 to 6000 ft)");

        if (!isMaxThrust && (altitude.Feet < 0 || altitude.Feet > 5000))
            throw new ArgumentOutOfRangeException(nameof(altitude), "3% Below, Altitude out of range (0 to 5000 ft)");


        double altK = altitude.Feet / 1000.0;
        var takeoffIndex = PerfCalculatorHelpers.BilinearInterpolate(
            isMaxThrust ? TakeOffIndexMaxThrust : TakeOffIndexThreePercentBelow,
            AxisTemps, AxisAlts, tempC, altK);

        return new TakeoffIndex(Math.Clamp(takeoffIndex, 4.0, 11.0));
    }


}
