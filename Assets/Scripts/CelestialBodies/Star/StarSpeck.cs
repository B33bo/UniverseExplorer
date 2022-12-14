using UnityEngine;
using UnityEngine.SceneManagement;
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
                canvas.transform.rotation = Quaternion.identity;
                canvas.transform.position = transform.position;
            }
        }

        public void Init(Star star)
        {
            c = Resources.Load<StarRenderer>("Objects/Star").colorGradient.Evaluate(((float)star.Temperature - 3000) / 7000);
            GetComponent<SpriteRenderer>().color = c;
            CameraFocus = false;

            if (isOriginal)
                starText.text = star.Name;
            else
                Destroy(canvas.gameObject);
        }
    }
}
