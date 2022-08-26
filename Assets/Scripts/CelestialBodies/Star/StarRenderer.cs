using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe.CelestialBodies.Planets
{
    public class StarRenderer : CelestialBodyRenderer
    {
        private Star TargetStar => Target as Star;

        public Gradient colorGradient;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private SpriteRenderer[] sunLines;

        [SerializeField]
        private SpriteRenderer Glow;

        [SerializeField]
        private MeshFilter[] sunSpots;

        private Vector2[] directions;

        [SerializeField]
        private SpriteMask spriteMask;

        public void ClearSunlines()
        {
            for (int i = 0; i < sunLines.Length; i++)
                Destroy(sunLines[i]);
            sunLines = new SpriteRenderer[0];
        }

        private void Start()
        {
            if (BodyManager.Parent is Star)
                Spawn(Vector2.zero, BodyManager.Parent.Seed);
            else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "SolarSystem")
                Spawn(Vector2.zero, null);

            SolarSystemSpawner.sun = Target as Star;
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new Star();

            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            transform.localScale = GetFairSize((float)Target.Width, (float)Star.MinSize, (float)Star.MaxSize) * Vector2.one;

            if (BodyManager.Parent is Star)
                transform.localScale *= 5;

            TargetStar.starColor = GetStarColor();

            directions = new Vector2[sunLines.Length];

            spriteRenderer.color = TargetStar.starColor;
            Glow.color = spriteRenderer.color;

            Color.RGBToHSV(spriteRenderer.color, out float H, out _, out _);
            Color lineColor = Color.HSVToRGB(H, 1, 1);

            for (int i = 0; i < sunLines.Length; i++)
            {
                float rot = RandomNum.GetFloat(360, Target.RandomNumberGenerator);
                directions[i] = new Vector2(Mathf.Cos(rot), Mathf.Sin(rot));
                sunLines[i].color = lineColor;
            }

            for (int i = 0; i < sunSpots.Length; i++)
            {
                Mesh pentagon = ShapeMaker.GetRegularShape(5, .1f);
                sunSpots[i].mesh = ShapeMaker.RandomizeMesh(pentagon, .15f, Target.RandomNumberGenerator);

                float rotation = RandomNum.GetFloat(360, Target.RandomNumberGenerator) * Mathf.Deg2Rad;
                float directionFromMid = RandomNum.GetFloat(.5f, Target.RandomNumberGenerator);
                sunSpots[i].transform.localPosition = new Vector3(Mathf.Cos(rotation), Mathf.Sin(rotation)) * directionFromMid;
            }

            //This is to do with sprite masks and sorting order
            //This happens so the weird texture doesn't clash with another star
            int sortingOrder = RandomNum.Get(0, 10000, Target.RandomNumberGenerator);
            spriteMask.frontSortingOrder = sortingOrder + 1;
            spriteMask.backSortingOrder = sortingOrder;

            for (int i = 0; i < sunLines.Length; i++)
            {
                sunLines[i].sortingOrder = sortingOrder;
            }
        }

        public Color GetStarColor()
        {
            return colorGradient.Evaluate((float)(TargetStar.Temperature - 3000) / 7000);
        }

        public override void OnUpdate()
        {
            for (int i = 0; i < sunLines.Length; i++)
            {
                sunLines[i].transform.position += (Vector3)directions[i] * Time.deltaTime;

                Vector2 localPos = sunLines[i].transform.localPosition;
                sunLines[i].transform.localPosition = new Vector3(localPos.x % .75f, localPos.y % .75f);
            }

            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * 5);
        }
    }
}
