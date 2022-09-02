using UnityEngine;
using Universe.CelestialBodies.Planets.Rocky;

namespace Universe
{
    public class RockPileGenerator : MonoBehaviour
    {
        [SerializeField]
        private BoulderRenderer prefab;

        public void Start()
        {
            //CameraControl.Instance.MyCamera.backgroundColor = ;
            if (!(BodyManager.Parent is RockPile rockPile))
            {
                rockPile = new RockPile();
                rockPile.SetSeed(0);
                rockPile.Create(Vector2.zero);
            }

            float[] totalColor = new float[3];
            float pos = 0;
            for (int i = 0; i < rockPile.Rocks; i++)
            {
                Color c = rockPile.colors[i];
                var newBoulder = Instantiate(prefab, new Vector3(0, pos), Quaternion.identity);
                newBoulder.SpawnBoulder(new Vector2(0, pos), rockPile[i], c, i);
                pos += 2;

                totalColor[0] += c[0];
                totalColor[1] += c[1];
                totalColor[2] += c[2];
            }

            BodyManager.InvokeSceneLoad(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            BodyManager.ReloadCommands();

            Color averageColor = new Color(totalColor[0] / rockPile.Rocks, totalColor[1] / rockPile.Rocks, totalColor[2] / rockPile.Rocks);
            CameraControl.Instance.MyCamera.backgroundColor = Color.white - averageColor;
        }
    }
}
