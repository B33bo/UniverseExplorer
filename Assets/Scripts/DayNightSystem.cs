using UnityEngine;
using Universe.CelestialBodies.Planets;
using Universe.CelestialBodies;
using System.Collections;
using Universe.CelestialBodies.Biomes;

namespace Universe
{
    public class DayNightSystem : MonoBehaviour
    {
        public float Time;
        private Planet p;
        [SerializeField]
        private Transform day, night;

        [SerializeField]
        private StarRenderer starRenderer;

        [SerializeField]
        private MoonRenderer moonRenderer;

        [SerializeField]
        private StarSpeck starSpeck;

        [SerializeField]
        private Transform rotator;

        [SerializeField]
        private SpriteRenderer sunrise;

        [SerializeField]
        private SkyBlackholeRenderer blackHoleRenderer;

        [SerializeField]
        private BlackHoleAccretionDiskRenderer blackHoleAccretionDiskPrefab;

        private TerrainGenerator terrainGenerator;

        private IEnumerator Start()
        {
            terrainGenerator = GameObject.FindObjectOfType<TerrainGenerator>();
            yield return new WaitForFrames(2);

            if (BodyManager.Parent is BlackHoleAccretionDisk)
            {
                LoadBlackHoleMoon();

                yield break;
            }
            else if (BodyManager.Parent is Moon moon)
            {
                p = moon.planet;

                System.Random rand = p.RandomNumberGenerator;
                var newPlanet = Instantiate(Resources.Load<CelestialBodyRenderer>(p.ObjectFilePos), day);
                newPlanet.Spawn(new Vector2(RandomNum.GetFloat(-10, 10, rand), RandomNum.GetFloat(-10, 10, rand)), moon.planet.Seed);
                newPlanet.CameraFocus = false;

                if (newPlanet.TryGetComponent(out UnityEngine.Rendering.SortingGroup sg))
                    Destroy(sg);

                newPlanet.GetComponent<SpriteRenderer>().sortingLayerName = "Sky";
                var sprites = newPlanet.GetComponentsInChildren<SpriteRenderer>();
                for (int i = 0; i < sprites.Length; i++)
                    sprites[i].sortingLayerName = "Sky";

                if (newPlanet.TryGetComponent(out SpriteMask spriteMask))
                {
                    spriteMask.backSortingLayerID = SortingLayer.NameToID("Sky");
                    spriteMask.frontSortingLayerID = SortingLayer.NameToID("Sky");
                }

                var particles = newPlanet.GetComponentsInChildren<ParticleSystemRenderer>();
                for (int i = 0; i < particles.Length; i++)
                    particles[i].sortingLayerName = "Default";

                var sortingGroups = newPlanet.GetComponentsInChildren<UnityEngine.Rendering.SortingGroup>();
                for (int i = 0; i < sortingGroups.Length; i++)
                    sortingGroups[i].sortingLayerName = "Sky";
            }
            else if (BodyManager.Parent is Continent c)
                p = c.planet;
            else
                p = BodyManager.Parent as Planet;

            if (p is null)
            {
                p = new ErrorPlanet();
                p.SetSeed(0);
                p.Create(Vector2.zero);
            }

            LoadDay();
            LoadNight();
        }

        private void LoadBlackHoleMoon()
        {
            p = new ErrorPlanet
            {
                moons = new Moon[0]
            };
            LoadNight();

            var blackHoleDisk = (BodyManager.Parent as BlackHoleAccretionDisk);

            blackHoleRenderer.gameObject.SetActive(true);
            blackHoleRenderer.Spawn(blackHoleDisk.blackHole);

            int moons = 10;
            if (blackHoleDisk.blackHole is SupermassiveBlackHole)
                moons = 20;

            for (int i = 0; i < moons; i++)
            {
                System.Random rand = new System.Random(i);
                var newBlackHoleMoon = Instantiate(blackHoleAccretionDiskPrefab, night);
                Vector2 pos = new Vector2(RandomNum.GetFloat(-30, 30, rand), RandomNum.GetFloat(-30, 10, rand));
                newBlackHoleMoon.Spawn(pos, null, blackHoleDisk.blackHole);
                newBlackHoleMoon.transform.position = pos;
                newBlackHoleMoon.GetComponent<SpriteRenderer>().sortingLayerName = "Sky";
                newBlackHoleMoon.transform.localScale *= 10;
                newBlackHoleMoon.CameraFocus = false;
            }
        }

        private void LoadDay()
        {
            if (p.sun is null)
            {
                p.sun = new Star();
                p.sun.SetSeed(0);
            }

            var newSun = Instantiate(starRenderer, day);
            newSun.Spawn(Vector2.zero, p.sun.Seed);

            var spriterenderer = newSun.GetComponent<SpriteRenderer>();
            spriterenderer.sortingLayerName = "Sky";
            spriterenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            newSun.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingLayerName = "Sky";
            newSun.ClearSunlines();
            newSun.CameraFocus = false;

            var children = newSun.GetComponentsInChildren<SpriteRenderer>();
            foreach (var child in children)
            {
                child.sortingLayerName = "Sky";
                child.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            }
        }

        private void LoadNight()
        {
            if (p.moons is null)
            {
                p.moons = new Moon[0];
            }

            for (int i = 0; i < p.moons.Length; i++)
            {
                if (BodyManager.Parent is Moon moon)
                {
                    if (p.moons[i] == moon)
                        continue;
                }
                var newMoon = Instantiate(moonRenderer, night);
                System.Random rand = new System.Random(p.moons[i].Seed + i);
                newMoon.Spawn(new Vector2(RandomNum.GetFloat(-10, 10, rand), RandomNum.GetFloat(-10, 10, rand)), p.moons[i].Seed);
                newMoon.CameraFocus = false;
                (newMoon.Target as Moon).planet = p;

                var spriteRenderer = newMoon.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingLayerName = "Sky";
                spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                var children = newMoon.GetComponentsInChildren<SpriteRenderer>();
                foreach (var child in children)
                    child.sortingLayerName = "Sky";
            }

            for (int i = 0; i < ObjectSpawner.starsLoaded.Count; i++)
            {
                if (ObjectSpawner.starsLoaded[i] == p.sun)
                    return;

                System.Random rand = new System.Random(ObjectSpawner.starsLoaded[i].Seed + i);
                var pos = new Vector2(RandomNum.GetFloat(-30, 30, rand), RandomNum.GetFloat(-10, 10, rand));
                var newSpeck = Instantiate(starSpeck, night);
                newSpeck.isOriginal = true;
                newSpeck.Spawn(pos, ObjectSpawner.starsLoaded[i].Seed);
            }

            for (int i = ObjectSpawner.starsLoaded.Count; i < 100; i++)
            {
                System.Random rand = new System.Random(p.Seed + i);
                var pos = new Vector2(RandomNum.GetFloat(-30, 30, rand), RandomNum.GetFloat(-30, 10, rand));
                var newSpeck = Instantiate(starSpeck, night);

                var newStar = new Star();
                newStar.Create(pos);
                newStar.Name = $"({newStar.Name})";
                newSpeck.Target = newStar;

                newSpeck.Init(newStar);
            }
        }

        private void Update()
        {
            transform.localScale = CameraControl.Instance.MyCamera.orthographicSize / 20 * Vector2.one;
            transform.position = (Vector2)CameraControl.Instance.transform.position;

            Time = (GlobalTime.Time % 360) / 180; //0 = day, 0.5 = sunset, 1 = night, 1.5 = sunrise
            rotator.transform.rotation = Quaternion.Euler(0, 0, GlobalTime.Time);

            float time = Time % 1;

            float alpha;
            if (time > .5f)
                alpha = 1 - (time - .5f) * 2;
            else
                alpha = time * 2;

            sunrise.color = new Color(1, 1, 1, alpha);
        }
    }
}
