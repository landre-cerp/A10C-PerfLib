namespace a10c_perf_lib.src;

public partial class PerfCalculator
{
    /// <summary>
    /// Seal Level Predicted Fan Speed (PTFS)
    /// Prediction of Fan Speed at Sea Level based on Temperature
    /// is used to estimate the available thrust and cancel takeoff if not enough thrust
    /// </summary>
    /// <param name="temp">T° in °C</param>
    /// <returns>Predicted Fan Speed (PTFS)</returns>
    public static double PTFS(double temperature)
    {
        if (temperature < -40 || temperature > 50)
            throw new ArgumentOutOfRangeException(nameof(temperature), "Temperature out of range (-40 to 50 °C)");

        double t = temperature;
        const double k = 84.49667731507539;
        const double a = -0.09989323265199185;
        const double b = -0.0010014770789936352;
        const double c = 0.000005170931496965901;
        return t switch
        {
            <= -30 => 0.1355 * t + 90.529,
            _ => k + t * (a + t * (b + t * c))
        };
    }

}
