# DCS A-10C Performance Library

C# library for A-10C Warthog performance calculations, intended for DCS World tooling.

## Namespace

All public types live in `a10c_perf_lib.src`.

## API Overview

### Takeoff

The full takeoff performance card is computed in one call once you have a takeoff index.

```csharp
using a10c_perf_lib.src;
using static a10c_perf_lib.src.PerfCalculator;

// 1. Optional: check predicted fan speed before computing index
double ptfs = PTFS(tempC: 25);  // → ~82.3 %

// 2. Compute pressure altitude from field elevation + altimeter
var qnh = new InHgQNH(29.65);
var pressureAlt = new PressureAltitude(feet: 1500, qnh);

// 3. Compute takeoff index (max thrust, or 3% below)
TakeoffIndex index = CalcTakeoffIndex(tempC: 25, pressureAlt, isMaxThrust: true);

// 4. Full takeoff performance card
TakeoffResult to = Takeoff(
    index:       index,
    grossWeight: new GrossWeight(38000),
    windKts:     10,           // positive = headwind
    flaps:       FLAPS.TO,
    thrust:      ThrustSetting.Max,
    rcr:         RCR.DRY
);

Console.WriteLine($"Ground run:       {to.GroundRunFt:F0} ft");
Console.WriteLine($"50 ft clearance:  {to.FiftyFtClearanceFt:F0} ft");
Console.WriteLine($"Critical field:   {to.CriticalFieldFt:F0} ft");
Console.WriteLine($"Takeoff speed:    {to.TakeoffSpeedKts:F1} kts");
Console.WriteLine($"Rotation speed:   {to.RotationSpeedKts:F1} kts");
```

**`TakeoffResult` properties**

| Property | Unit | Description |
|---|---|---|
| `GroundRunFt` | ft | Ground roll to lift-off |
| `FiftyFtClearanceFt` | ft | Distance to clear a 50 ft obstacle |
| `CriticalFieldFt` | ft | Minimum runway length (balanced field) |
| `TakeoffSpeedKts` | kts | V2 — lift-off speed |
| `RotationSpeedKts` | kts | V1 — rotation speed (V2 − 10 kts) |

**Input ranges**

| Parameter | Range |
|---|---|
| Temperature | −40 to 50 °C |
| Pressure altitude | 0 to 6 000 ft |
| Gross weight | 25 629 to 46 476 lbs |
| Wind | negative = tailwind |
| Flaps | `FLAPS.TO` (7°) or `FLAPS.UP` (0°) |
| Thrust | `ThrustSetting.Max` or `ThrustSetting.ThreePercentBelow` |
| RCR | `RCR.DRY` (23), `RCR.WET` (12), `RCR.ICY` (5) |

---

### Climb

```csharp
ClimbResult climb = Climb(
    startAltFt:  25000,
    targetAltFt: 40000,
    grossWeight: 35000,
    drag:        0,        // drag index 0–8
    deltaISA:    15        // optional ISA deviation in °C
);

Console.WriteLine($"Distance: {climb.DistanceNm:F1} NM");
Console.WriteLine($"Fuel:     {climb.FuelLbs:F0} lbs");
Console.WriteLine($"Time:     {climb.TimeMin:F0} min");
```

**Input ranges:** altitude 25 000–50 000 ft, gross weight 25 000–50 000 lbs, drag 0–8.

---

### Descent

```csharp
DescentResult desc = Descent(
    startAltFt:  35000,
    endAltFt:    5000,
    grossWeight: 32000,
    drag:        0
);

Console.WriteLine($"Distance:        {desc.DistanceNm:F0} NM");
Console.WriteLine($"Fuel:            {desc.FuelLbs:F0} lbs");
Console.WriteLine($"Time:            {desc.TimeMin:F0} min");
Console.WriteLine($"Max range speed: {desc.MaxRangeSpeedKts:F0} kts");

// Max-range descent speed is also available standalone:
double speed = MaxRangeDescentSpeed(grossWeight: 32000, drag: 0);
```

**Input ranges:** start altitude 0–35 000 ft, gross weight 25 000–50 000 lbs, drag 0–8.

---

### Utilities

```csharp
// QNH — standard presets or explicit values
QNH std   = QNH.StdInHg;           // 29.92 inHg
QNH qnh   = new InHgQNH(29.65);
QNH qnhSI = new HpaQNH(1005);

// Pressure altitude from field elevation + altimeter
var pa = new PressureAltitude(feet: 2000, new InHgQNH(29.65));

// RCR without anti-skid
double rcrNoAS = PerfCalculator.RCRWithoutAntiSkid(RCR.DRY);  // → 15.985
```

## Project Layout

```
PerformanceLib/src/
├── PerfCalculator.cs / .Data.cs / .Helpers.cs / .PTFS.cs
├── CorrectionTable.cs          — public abstract interpolation base
├── Types/                      — GrossWeight, TakeoffIndex, PressureAltitude, QNH,
│                                 TakeoffResult, ClimbResult, DescentResult
├── Takeoff/                    — PerfCalculator partials for takeoff phase
├── Cruise/                     — PerfCalculator partials for climb & descent
└── Tables/                     — internal bilinear interpolation tables
    ├── Takeoff/ (+ FiftyFt/)
    ├── Climb/
    └── Descent/
```
