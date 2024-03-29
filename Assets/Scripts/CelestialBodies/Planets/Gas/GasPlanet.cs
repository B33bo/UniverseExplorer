using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets
{
    public class GasPlanet : Planet
    {
        public const float ContrastSatDropoff = .33f;
        public const double MinScale = 5000 * Measurement.Km, MaxScale = 120000 * Measurement.Km;
        public const double MinMass = 3e22 * Measurement.Kg, MaxMass = 4e24 * Measurement.Kg;
        public override string ObjectFilePos => "Planets/Gas";

        public override string TypeString => "Gas Planet";

        public override string PlanetTargetScene => "GasPlanet";

        [InspectableVar("Color")]
        public ColorHSV GasColor;

        [InspectableVar("Contrast", Params = new object[] { 0f, 1f })]
        public float Contrast;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            switch (Seed)
            {
                default:
                    Name = GenerateName();
                    Radius = RandomNum.Get(MinScale, MaxScale, RandomNumberGenerator);
                    Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
                    GasColor = RandomNum.GetColor(RandomNumberGenerator);
                    Contrast = RandomNum.GetFloat(0, .4f, RandomNumberGenerator);
                    break;
                case Star.Jupiter:
                    Name = "Jupiter";
                    Radius = 69911 * Measurement.Km;
                    Mass = 1.898e27 * Measurement.Kg;
                    GasColor = new ColorHSV(27 / 360f, .35f, .58f);
                    Contrast = .1f;
                    break;
                case Star.Saturn:
                    Name = "Saturn";
                    Radius = 58232 * Measurement.Km;
                    Mass = 5.683e36 * Measurement.Kg;
                    GasColor = new ColorHSV(40 / 360f, .33f, .81f);
                    Contrast = .02f;
                    break;
                case Star.Uranus:
                    Name = "Uranus";
                    Radius = 25362 * Measurement.Km;
                    Mass = 8.681e25 * Measurement.Kg;
                    GasColor = new ColorHSV(186 / 360f, .44f, .81f);
                    Contrast = .02f;
                    break;
                case Star.Neptune:
                    Name = "Neptune";
                    Radius = 24622 * Measurement.Km;
                    Mass = 1.024e26 * Measurement.Kg;
                    GasColor = new ColorHSV(234 / 360f, .82f, .81f);
                    Contrast = .1f;
                    break;
            }
        }

        public override string GetBonusTypes()
        {
            return "Color - " + GasColor.ToHumanString();
        }
    }
}
