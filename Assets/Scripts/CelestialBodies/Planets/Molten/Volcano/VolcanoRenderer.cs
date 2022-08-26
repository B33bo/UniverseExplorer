using UnityEngine;
using Universe.CelestialBodies.Planets;
using Universe.CelestialBodies.Planets.Molten;

namespace Universe
{
    public class VolcanoRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private SpriteRenderer Rock;

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Volcano();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            pos.y += RandomNum.GetFloat(0, 3.5f, Target.RandomNumberGenerator);
            Target.Position = pos;
            
            if (BodyManager.Parent is Planet p)
                Rock.color = TerrainGenerator.Instance.BiomeAtPosition(Target.Position.x).groundColor;
        }
    }
}
