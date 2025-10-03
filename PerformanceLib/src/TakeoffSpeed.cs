namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    public static double TakeOffSpeed(double Grossweight)
    {
        return TakeOffSpeed(Grossweight, FLAPS.TO);
    }

    /// <summary>
    /// Take Off Speed in Knots
    /// </summary>
    /// <param name="Grossweight">Grossweight in Lbs</param>
    /// <returns>Speed in Knots</returns>
    public static double TakeOffSpeed(double Grossweight, FLAPS flaps)
    {
        
        if (Grossweight < EMPTY_WEIGHT_LBS || Grossweight > MAX_TAKEOFF_WEIGHT_LBS)
            throw new ArgumentOutOfRangeException(nameof(Grossweight), $"Grossweight out of range ({EMPTY_WEIGHT_LBS} to {MAX_TAKEOFF_WEIGHT_LBS} lbs)");

        double w = Grossweight;

        if (flaps == FLAPS.UP)
            return 52.4 + w * (2.67e-3 + w * -1.1e-8);
        
        return 43.8 + w * (3.11e-3 + w * (-2.38e-8 + w * 1.08e-13));
    }

    /// <summary>
    /// Get the rotation speed for Takeoff with flaps TO (7°)
    /// </summary>
    /// <param name="grossWeight">Grossweight in lbs</param>
    /// <returns>Speed in Knots</returns>
    public static double RotationSpeed(double grossWeight)
    {
        return RotationSpeed(grossWeight, FLAPS.TO);
    }

    /// <summary>
    /// Rotation Speed on a specific flaps configuration
    /// </summary>
    /// <param name="grossWeight">Gross weight in pounds</param>
    /// <param name="flaps">Flaps configuration</param>
    /// <returns>Rotation speed in knots</returns>
    public static double RotationSpeed(GrossWeight grossWeight, FLAPS flaps)
    {
        if (FLAPS.UP != flaps && FLAPS.TO != flaps)
            throw new ArgumentOutOfRangeException(nameof(flaps), "Rotation speed is not defined this Flaps configuration");

        return RotationSpeed(grossWeight.Value, flaps);
    }

    /// <summary>
    /// Rotation Speed in Knots is approximately 10 knots less than Takeoff Speed
    /// </summary>
    /// <param name="grossWeight">Grossweight in lbs</param>
    /// <returns>Rotation speed known as V1</returns>
    public static double RotationSpeed(double grossWeight, FLAPS flaps)
    {
        return TakeOffSpeed(grossWeight, flaps) - 10;
    }

    /// <summary>
    /// Takeoff Index from Temp (°C) and Altitude (ft)
    /// </summary>
    /// <param name="tempC">T° in °C</param>
    /// <param name="altitude">Pressure Altitude in Feet</param>
    /// <param name="flaps">Flaps configuration, only TO (TakeOff) is supported</param>
    /// <returns></returns>

}
