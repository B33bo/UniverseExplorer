using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class Rainbow : CelestialBody
    {
        public override string TypeString => "Rainbow";

        public override string TravelTarget => "Lights";

        public override bool Circular => true;

        [InspectableVar("Radius")]
        public float physicalRadius;

        [InspectableVar("Alpha")]
        public float alpha;

        [InspectableVar("Ring Size")]
        public float ringSize;

        public override void Create(Vector2 pos)
        {
            Name = "Rainbow";
            Position = pos;
            physicalRadius = RandomNum.GetFloat(3, 12, RandomNumberGenerator);
            Radius = physicalRadius * Measurement.Km;
            alpha = RandomNum.GetFloat(1, RandomNumberGenerator);
            ringSize = RandomNum.GetFloat(.6f, .9f, RandomNumberGenerator);
        }

        public void SetIntensity(float intensity)
        {
            physicalRadius = intensity * 35f;
            Radius = physicalRadius * Measurement.Km;
            alpha = intensity;
            ringSize = Mathf.Min(1.4f - intensity, .9f);
        }
    }
}
