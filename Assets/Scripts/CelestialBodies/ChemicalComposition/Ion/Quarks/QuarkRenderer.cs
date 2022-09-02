using UnityEngine;

namespace Universe
{
    public class QuarkRenderer : CelestialBodyRenderer
    {
        public string Type, Name, Symbol;
        public int PreonConfig, AntiPreonConfig;

        public bool Create;
        public int SeedModifier;

        [SerializeField]
        private TMPro.TextMeshProUGUI text;

        private void Start()
        {
            if (Create)
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
            quark.Create(pos);

            text.text = Symbol;
        }
    }
}
