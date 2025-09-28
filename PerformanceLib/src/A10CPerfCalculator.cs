namespace a10c_perf_lib.src;

public partial class A10CPerfCalculator : IAircraftPerformanceCalculator
{
    public const double EMPTY_WEIGHT_LBS = 25629;
    public const double MAX_TAKEOFF_WEIGHT_LBS = 46476;

    /// <summary>
    /// Seal Level Predicted Fan Speed (PTFS)
    /// Prediction of Fan Speed at Sea Level based on Temperature
    /// is used to estimate the available thrust and cancel takeoff if not enough thrust
    /// </summary>
    /// <param name="temp">T° in °C</param>
    /// <returns>Predicted Fan Speed (PTFS)</returns>
    public double PTFS(double temperature)
    {
        if (temperature < -40 || temperature > 50)
            throw new ArgumentOutOfRangeException(nameof(temperature), "Temperature out of range (-40 to 50 °C)");

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

    public double RequiredFanSpeed(double temperature,PressureAltitude alt, double grossWeight)
    {
        return 88.0 - 0.1 * (temperature - PerfCalculatorHelpers.StandardTemperature(alt)) + 0.0005 * (grossWeight - 22000);

    }

    /// <summary>
    /// Take Off Speed in Knots
    /// </summary>
    /// <param name="Grossweight">Grossweight in Lbs</param>
    /// <returns>Speed in Knots</returns>
    public double TakeOffSpeed(double Grossweight)
    {
        if (Grossweight < EMPTY_WEIGHT_LBS || Grossweight > MAX_TAKEOFF_WEIGHT_LBS)
            throw new ArgumentOutOfRangeException(nameof(Grossweight), $"Grossweight out of range ({EMPTY_WEIGHT_LBS} to {MAX_TAKEOFF_WEIGHT_LBS} lbs)");
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
        if (tempC < -30 || tempC > 50)
            throw new ArgumentOutOfRangeException(nameof(tempC), "Temperature out of range (-30 to 50 °C)");
        if (altitude.Feet < 0 || altitude.Feet > 6000)
            throw new ArgumentOutOfRangeException(nameof(altitude), "Altitude out of range (0 to 6000 ft)");

        double altK = altitude.Feet / 1000.0;
        var takeoffIndex = PerfCalculatorHelpers.BilinearInterpolate(TakeOffIndexMaxThrust, Temps, Alts, tempC, altK);
        
        return Math.Clamp(takeoffIndex, 4.0, 11.0);
    }

    public enum FLAPS
    {
        UP = 0,
        TO = 7,
    }

    public enum RCR
    {
        DRY = 23,
        WET = 12,
        ICY = 5,
    }

    public static double RCRWithoutAntiSkid(RCR rcr)
    {
        return 0.695 * (double)rcr;
    }

}
