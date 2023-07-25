using UnityEngine;
using Universe.CelestialBodies;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class StarSpeck : CelestialBodyRenderer
    {
        private Color c;

        [SerializeField]
        private TMPro.TextMeshProUGUI starText;

        [SerializeField]
        private Transform canvas;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public bool isOriginal;

        public override void Spawn(Vector2 pos, int? seed)
        {
            var star = new Star();
            Target = star;
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            starText.text = Target.Name;
            Target.Create(pos);

            Init(star);
        }

        public override void OnUpdate()
        {
            if (isOriginal)
            {
                canvas.gameObject.SetActive(Input.GetKey(KeyCode.LeftShift) && isOriginal);
                canvas.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            }
        }

        public void Init(Star star)
        {
            c = CelestialBodies.Universe.LoadedInfo.BlackBodyRadiation.Evaluate(((float)star.Temperature - Star.minTemp) / (Star.maxTemp - Star.minTemp));
            GetComponent<SpriteRenderer>().color = c;
            CameraFocus = false;

            if (isOriginal)
                starText.text = star.Name;
            else
                Destroy(canvas.gameObject);
        }

        public void SetAlpha(float value)
        {
            var color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, value);
        }
    }
}
