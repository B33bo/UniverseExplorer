using Btools.DevConsole;
using UnityEngine;
using Universe.CelestialBodies.Planets.Error;

namespace Universe
{
    public class EndIsNighRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private bool positive;

        [SerializeField]
        private bool create;

        [SerializeField]
        private float yPos = 4;

        private void Start()
        {
            if (create)
                Spawn(new Vector2(positive ? EndIsNighSign.TheEnd - 16 : -EndIsNighSign.TheEnd + 16, yPos), null);

            if (positive)
            {
                DevCommands.RegisterVar(new DevConsoleVariable("terrainend", "The end of terrain generation", typeof(int), () => EndIsNighSign.TheEnd.ToString()));
                DevCommands.RegisterVar(new DevConsoleVariable("-terrainend", "The negative end of terrain generation", typeof(int), () => (-EndIsNighSign.TheEnd).ToString()));
            }
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new EndIsNighSign();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);
        }
    }
}
