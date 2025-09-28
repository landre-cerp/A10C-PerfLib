using a10c_perf_lib.src;

namespace A10CPerfTests
{
    public class CommonToolsTests
    {
        // Tests unitaires pour les fonctions de PerfCalculatorHelpers

        [Theory]
        [InlineData(0, 1013.25, 0)] // Sea level standard
        [InlineData(500, 1018.6, 340)] 
        [InlineData(1000, 1023.9, 680)]
        public void GetPressureAltitude_ReturnsExpected(
            double indicatedAltitudeFt, 
            double hpa,
            double expected)
        {
            var result = new PressureAltitude(indicatedAltitudeFt, new HpaQNH(hpa));
            Assert.InRange(Math.Ceiling(result.Feet), expected -1 , expected +1);
        }

        [Fact]
        public void Standard_Qnh_should_compare()
        {
            var qnh1 = new InHgQNH(QNH.STD_INHG);
            var qnh2 = new HpaQNH(QNH.STD_HPA);
            Assert.Equal(qnh1.InHg, qnh2.InHg, 2);
            Assert.Equal(qnh1.HPa, qnh2.HPa, 0);
        }
    }
}
