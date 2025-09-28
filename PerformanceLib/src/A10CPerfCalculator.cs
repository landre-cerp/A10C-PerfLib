namespace a10c_perf_lib.src;

public partial class A10CPerfCalculator : IAircraftPerformanceCalculator
{
    /// <summary>
    /// Seal Level Predicted Fan Speed (PTFS)
    /// Prediction of Fan Speed at Sea Level based on Temperature
    /// is used to estimate the available thrust and cancel takeoff if not enough thrust
    /// </summary>
    /// <param name="temp">T° in °C</param>
    /// <returns>Predicted Fan Speed (PTFS)</returns>
    public double PTFS(double temperature)
    {
        double t=temperature;
        const double k = 84.49667731507539;
        const double a = -0.09989323265199185;
        const double b = -0.0010014770789936352;
        const double c = 0.000005170931496965901;
        return t switch
        {
            <= -30 => 0.1355 * t + 90.529,
            _ => k + t * (a + t* (b + t*c))
        };
    }

    /// <summary>
    /// Take Off Speed in Knots
    /// </summary>
    /// <param name="Grossweight">Grossweight in Lbs</param>
    /// <returns>Speed in Knots</returns>
    public double TakeOffSpeed(double Grossweight)
    {
        double w = Grossweight;
        return 43.8 + w * (3.11e-3 + w * (-2.38e-8 + w * 1.08e-13));
    }

    /// <summary>
    /// Rotation Speed in Knots is approximately 10 knots less than Takeoff Speed
    /// </summary>
    /// <param name="grossWeight">Grossweight in lbs</param>
    /// <returns>Rotation speed known as V1</returns>
    public double RotationSpeed(double grossWeight)
    {
        return TakeOffSpeed(grossWeight) - 10;
    }

    /// <summary>
    /// Takeoff Index from Temp (°C) and Altitude (ft)
    /// </summary>
    /// <param name="tempC">T° in °C</param>
    /// <param name="altitude">Pressure Altitude in Feet</param>
    /// <param name="flaps">Flaps configuration, only TO (TakeOff) is supported</param>
    /// <returns></returns>

    public static double GetTakeoffIndex(double tempC, PressureAltitude altitude, FLAPS flaps)
    {
        if (flaps != FLAPS.TO)
            throw new ArgumentException("Flaps configuration not yet handled");

        double altK = altitude.Feet / 1000.0;

        return PerfCalculatorHelpers.BilinearInterpolate(TakeOffIndexMaxThrust, Temps, Alts, tempC, altK);
    }

    public enum RCR
    {
        DRY = 23,
        WET = 12,
        ICY = 5,
    }

    public enum FLAPS
    {
        UP = 0,
        TO = 7,
    }

    public static double RCRWithoutAntiSkid(RCR rcr)
    {
        return 0.695 * (double)rcr + 6.15e-3;
    }

}
