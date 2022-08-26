using UnityEngine;
using Universe.CelestialBodies.Biomes.Desert;

namespace Universe
{
    public class TumbleWeedRenderer : CelestialBodyRenderer
    {
        public override void Spawn(Vector2 pos, int? seed)
        {
            var tumbleWeed = new TumbleWeed();
            Target = tumbleWeed;

            if (seed.HasValue)
                Target.SetSeed(seed.Value);

            Target.Create(pos);

            transform.rotation = Quaternion.Euler(0, 0, (float)tumbleWeed.rotation);
            transform.localScale = Vector3.one * ((float)tumbleWeed.Radius / (float)Measurement.M);
        }
    }
}
