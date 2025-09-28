namespace a10c_perf_lib.src;

public readonly struct PressureAltitude
{
    public double Feet { get; }

    public PressureAltitude(double feet, QNH qnh)
    {
        Feet = feet + (QNH.STD_HPA - qnh.HPa) * 30;
    }

    public override string ToString() => $"{Feet} ft";

}