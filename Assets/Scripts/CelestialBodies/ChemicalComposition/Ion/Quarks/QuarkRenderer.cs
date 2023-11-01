using UnityEngine;

namespace Universe
{
    public class QuarkRenderer : CelestialBodyRenderer
    {
        public string Type, Name, Symbol;

        [SerializeField]
        private string charge, spin;

        [SerializeField]
        private float mass;

        public bool Create;
        public int SeedModifier;

        [SerializeField]
        private TMPro.TextMeshProUGUI text;

        private void Start()
        {
            if (!Create)
                return;
            Spawn(transform.position, BodyManager.GetSeed() + SeedModifier);
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Quark quark = new Quark();
            Target = quark;
            if (seed.HasValue)
                quark.SetSeed(seed.Value);

            quark.Name = Name;
            quark.Symbol = Symbol;
            quark.Type = Type;

            quark.Charge = charge;
            quark.Spin = spin;
            quark.Mass = mass * Measurement.evC2;

            quark.Create(pos);

            text.text = Symbol;
        }
    }
}
