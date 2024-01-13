using Btools.DevConsole;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Universe.CelestialBodies;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class DayNightSystem : MonoBehaviour
    {
        public static DayNightSystem Instance { get; private set; }
        public static float LightIntensity = 1;

        public float Time;
        private Planet planet;

        [SerializeField]
        private Transform day, night;

        [SerializeField]
        private Light2D dayNightLight;

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

        [SerializeField]
        private bool FreezeTime;

        public Color dayColor, nightColor;

        private StarSpeck[] starSpecks;

        [SerializeField]
        private float brightnessAtNight = .1f;

        private void Awake()
        {
            Instance = this;
            DevCommands.RegisterVar(new DevConsoleVariable("PlanetTime", "Time on the planet", typeof(float), () => Time, x => Time = float.Parse(x)));
            DevCommands.Register("FreezeTime", "Freeze Time on planet", _ => { FreezeTime = !FreezeTime; return FreezeTime.ToString(); });
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        private IEnumerator Start()
        {
            yield return new WaitForFrames(2);

            if (BodyManager.Parent is BlackHoleAccretionDisk)
            {
                LoadBlackHoleMoon();
                yield break;
            }
            else if (BodyManager.Parent is Moon moon && moon.planet != null)
            {
                planet = moon.planet;
                SpawnAsMoon(moon);
            }
            else if (BodyManager.Parent is Planet parentPlanet)
                planet = parentPlanet;

            // don't make it an else statement as other statements above could fail
            if (planet == null)
            {
                planet = new ErrorPlanet();
                planet.SetSeed(0);
                planet.Create(Vector2.zero);
            }

            LoadDay();
            LoadNight();
        }

        private void SpawnAsMoon(Moon moon)
        {
            System.Random rand = planet.RandomNumberGenerator;

            var newPlanet = Instantiate(ObjectPaths.Instance.GetCelestialBody(planet.ObjectFilePos), day);
            newPlanet.Spawn(new Vector2(RandomNum.GetFloat(-10f, 10, rand), RandomNum.GetFloat(-10, 10, rand)), moon.planet.Seed);
            Destroy(newPlanet.GetComponent<Collider2D>());

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

        private Color GetCameraColor()
        {
            if (Time > 1)
                return Color.Lerp(nightColor, dayColor, Time - 1);
            return Color.Lerp(dayColor, nightColor, Time);
        }

        private void LoadBlackHoleMoon()
        {
            planet = new ErrorPlanet
            {
                moons = new MoonRenderer[0]
            };
            LoadNight();

            var blackHoleDisk = (BodyManager.Parent as BlackHoleAccretionDisk);

            blackHoleRenderer.gameObject.SetActive(true);
            blackHoleRenderer.Spawn(blackHoleDisk.blackHole);

            Destroy(blackHoleRenderer.GetComponent<Collider2D>());

            int moons = 10;
            if (blackHoleDisk.blackHole is SupermassiveBlackHole)
                moons = 20;

            for (int i = 0; i < moons; i++)
            {
                System.Random rand = new System.Random(i);
                Vector2 pos = new(RandomNum.GetFloat(-30, 30, rand), RandomNum.GetFloat(-30, 10, rand));

                var newBlackHoleMoon = Instantiate(blackHoleAccretionDiskPrefab, night);
                newBlackHoleMoon.Spawn(pos, null, blackHoleDisk.blackHole);
                newBlackHoleMoon.transform.position = pos;
                newBlackHoleMoon.transform.localScale *= 10;

                newBlackHoleMoon.GetComponent<SpriteRenderer>().sortingLayerName = "Sky";
                newBlackHoleMoon.CameraFocus = false;

                Destroy(newBlackHoleMoon.GetComponent<Collider2D>());
            }
        }

        private void LoadDay()
        {
            if (planet.sun is null)
            {
                planet.sun = new Star();
                planet.sun.SetSeed(0);
            }

            var newSun = Instantiate(starRenderer, day);
            newSun.Spawn(Vector2.zero, planet.sun.Seed);

            var spriterenderer = newSun.GetComponent<SpriteRenderer>();
            spriterenderer.sortingLayerName = "Sky";
            spriterenderer.maskInteraction = SpriteMaskInteraction.None;

            newSun.GetComponent<UnityEngine.Rendering.SortingGroup>().sortingLayerName = "Sky";
            newSun.CameraFocus = false;

            Destroy(newSun.GetComponent<Collider2D>());

            var children = newSun.GetComponentsInChildren<SpriteRenderer>();
            foreach (var child in children)
            {
                child.sortingLayerName = "Sky";
                child.maskInteraction = SpriteMaskInteraction.None;
            }

            if (planet.sun.StrangeStar)
                dayColor = .2f * planet.sun.StarColor + .8f * dayColor;
        }

        private void LoadNight()
        {
            planet.moons ??= new MoonRenderer[0];
            for (int i = 0; i < planet.moons.Length; i++)
            {
                if (BodyManager.Parent is Moon moon && planet.moons[i].Target.Seed == moon.Seed)
                    // don't spawn self as a moon lol
                    continue;

                var newMoon = Instantiate(moonRenderer, night);
                System.Random rand = new System.Random(planet.moons[i].Target.Seed + i);
                newMoon.Spawn(new Vector2(RandomNum.GetFloat(-10, 10, rand), RandomNum.GetFloat(-10, 10, rand)), planet.moons[i].Target.Seed);
                newMoon.CameraFocus = false;
                (newMoon.Target as Moon).planet = planet;

                var spriteRenderer = newMoon.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingLayerName = "Sky";
                spriteRenderer.maskInteraction = SpriteMaskInteraction.None;

                var children = newMoon.GetComponentsInChildren<SpriteRenderer>();
                Destroy(newMoon.GetComponent<Collider2D>());

                foreach (var child in children)
                    child.sortingLayerName = "Sky";
            }

            const int starCount = 200;
            starSpecks = new StarSpeck[starCount];

            for (int i = 0; i < starCount; i++)
            {
                var newSpeck = Instantiate(starSpeck, night);

                if (i < ObjectSpawner.starsLoaded.Count && ObjectSpawner.starsLoaded[i] != planet.sun)
                {
                    int seed = ObjectSpawner.starsLoaded[i].Seed;
                    var rand = new System.Random(seed);
                    rand.Next();
                    Vector2 pos = new(RandomNum.GetFloat(-50, 50, rand), RandomNum.GetFloat(-50, 50, rand));
                    newSpeck.isOriginal = true;
                    newSpeck.Spawn(pos, ObjectSpawner.starsLoaded[i].Seed);
                }
                else
                {
                    int seed = planet.Seed + (i - ObjectSpawner.starsLoaded.Count);
                    System.Random rand = new(seed);
                    Vector2 pos = new(RandomNum.GetFloat(-50, 50, rand), RandomNum.GetFloat(-50, 50, rand));

                    var newStar = new Star();
                    newStar.SetSeed(seed);
                    newStar.Create(pos);
                    newStar.Name = $"({newStar.Name})";
                    newSpeck.Target = newStar;

                    newSpeck.Init(newStar);
                }
                starSpecks[i] = newSpeck;
            }
        }

        private void Update()
        {
            CameraControl.Instance.MyCamera.backgroundColor = GetCameraColor();
            transform.localScale = CameraControl.Instance.MyCamera.orthographicSize / 20 * Vector2.one;
            transform.position = new Vector2(CameraControl.Instance.Position.x,
                Mathf.Max(CameraControl.Instance.CameraBounds.yMin, 2));

            if (!FreezeTime)
                Time = GetTime(); // 0 = day, 0.5 = sunset, 1 = night, 1.5 = sunrise
            rotator.transform.rotation = Quaternion.Euler(0, 0, Time * 180);

            Brightness();
            Sunrise();
            Stars();
        }

        private void Brightness()
        {
            // (1-brightness)(x-1)^2 + brightnessAtNight

            LightIntensity = (1 - brightnessAtNight) * (Time - 1) * (Time - 1) + brightnessAtNight;
            dayNightLight.intensity = LightIntensity;
        }

        private void Sunrise()
        {
            float time = Time % 1;

            float alpha;
            if (time > .5f)
                alpha = 1 - (time - .5f) * 2;
            else
                alpha = time * 2;

            sunrise.color = new Color(1, 1, 1, alpha * .7f);
        }

        private void Stars()
        {
            if (starSpecks is null)
                return;
            for (int i = 0; i < starSpecks.Length; i++)
            {
                if (Time > 1f)
                    starSpecks[i].SetAlpha(.5f - (Time - 1f));
                else
                    starSpecks[i].SetAlpha(Time);
            }
        }

        private float GetTime()
        {
            return GlobalTime.Time % 360 / 180; // 0 = day, 0.5 = sunset, 1 = night, 1.5 = sunrise
        }
    }
}
