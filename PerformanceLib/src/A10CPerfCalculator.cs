namespace a10c_perf_lib.src;

public partial class A10CPerfCalculator : IAircraftPerformanceCalculator
{
    /// <summary>
    /// Seal Level Predicted Fan Speed (PTFS)
    /// </summary>
    /// <param name="temp">T° in °C</param>
    /// <returns>Predicted Fan Speed (PTFS)</returns>
    public double PTFS(double temp)
    {
        return temp switch
        {
            <= -30 => 0.1355 * temp + 90.529,
            _ => 84.49667731507539 - 0.09989323265199185 * temp - 0.0010014770789936352 * temp * temp + 0.000005170931496965901 * Math.Pow(temp, 3)
        };
    }

    /// <summary>
    /// Take Off Speed in Knots
    /// </summary>
    /// <param name="grossWeight">Gross Weight in Lbs</param>
    /// <returns>Speed in Knots</returns>
    public double TakeOffSpeed(double grossWeight)
    {
        return 43.8 +( 3.11e-3 * grossWeight) - (2.38e-8 * Math.Pow(grossWeight,2)) + (1.08e-13 * Math.Pow(grossWeight,3));
    }

    /// <summary>
    /// Takeoff Index from Temp (°C) and Altitude (ft)
    /// </summary>
    /// <param name="tempC">T° in °C</param>
    /// <param name="pressure_altFt">Pressure Altitude in Feet</param>
    /// <returns></returns>

    public static double GetTakeoffIndex(double tempC, double pressure_altFt)
    {
        double altK = pressure_altFt / 1000.0;

        return PerfCalculatorHelpers.BilinearInterpolate(TakeOffIndexMaxThrust, Temps, Alts, tempC, altK);
    }
}
