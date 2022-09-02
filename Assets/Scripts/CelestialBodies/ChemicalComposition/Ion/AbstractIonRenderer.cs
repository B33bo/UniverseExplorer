using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class AbstractIonRenderer : CelestialBodyRenderer
    {
        private Ion ion;

        [SerializeField]
        private string ionName;

        [SerializeField]
        private TMPro.TextMeshProUGUI text;

        private void Start()
        {
            ion = Ion.FindIon(ionName);
            Spawn((Vector2)transform.localPosition, null, ion);

            text.SetText(ionName switch
            {
                "Ee" => "e-",
                "Nn" => "",
                "Pp" => "p+",
                _ => ionName,
            });
        }

        private void OnValidate()
        {
            if (text is null)
                return;
            text.SetText(ionName);
            name = ionName;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new IonObject();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            (Target as IonObject).IsAbstract = true;
        }

        public void Spawn(Vector2 pos, int? seed, Ion ion)
        {
            IonObject ionObj = new IonObject();
            Target = ionObj;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            ionObj.Create(pos, ion);
            this.ion = ion;
        }
    }
}
