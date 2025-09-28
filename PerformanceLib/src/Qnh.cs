using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a10c_perf_lib.src;

public enum QNH_Unit
{
    inHg,
    hPa
}

public class QNH
{
    public const double STD_INHG = 29.92;
    public const double STD_HPA = 1013.25;

    public static QNH StdInHg { get; } = new QNH(STD_INHG, QNH_Unit.inHg);
    public static QNH StdHpa { get; } = new QNH(STD_HPA, QNH_Unit.hPa);

    public double Value { get; }
    public QNH_Unit Unit { get; }

    public double InHg
    {
        get
        {
            if (Unit == QNH_Unit.inHg)
                return Value;
            // Conversion hPa -> inHg
            return Math.Round(Value / STD_HPA * STD_INHG, 2);
        }
    }

    public double HPa
    {
        get
        {
            if (Unit == QNH_Unit.hPa)
                return Value;
            // Conversion inHg -> hPa
            return Math.Round(Value / STD_INHG * STD_HPA);
        }
    }

    public QNH(double value, QNH_Unit unit)
    {
        Value = value;
        Unit = unit;
    }
}

public class HpaQNH : QNH
{
    public HpaQNH(double hpa) : base(hpa, QNH_Unit.hPa) { }
}    

public class InHgQNH : QNH
{
    public InHgQNH(double inHg) : base(inHg, QNH_Unit.inHg) { }
}