using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Universe.CelestialBodies.Planets.Gas;

namespace Universe
{
    public class GasPlanetObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private JetStream jetStream;

        const float sizeOfJet = 1.5f;

        private Dictionary<float, JetStream> jetStreams = new Dictionary<float, JetStream>();

        private void Start()
        {
            CameraControl.Instance.OnPositionUpdate += ResetCells;
        }

        private void ResetCells(Rect cameraRect)
        {
            var newCells = PositionsOnScreen(cameraRect);

            var keys = jetStreams.Keys.ToArray();
            foreach (var yPos in keys)
            {
                if (newCells.Contains(yPos))
                    continue;
                Destroy(jetStreams[yPos].gameObject);
                jetStreams.Remove(yPos);
            }

            for (int i = 0; i < newCells.Count; i++)
            {
                if (keys.Contains(newCells[i]))
                    continue;
                SpawnAt(newCells[i]);
            }
        }

        private void SpawnAt(float y)
        {
            var newObj = Instantiate(jetStream, new Vector3(0, y), jetStream.transform.rotation);
            jetStreams.Add(y, newObj);

            if (BodyManager.Parent is null)
                newObj.Seed = (int)y;
            else
                newObj.Seed = (int)y * BodyManager.Parent.Seed;
        }

        private List<float> PositionsOnScreen(Rect rect)
        {
            List<float> value = new List<float>();
            float start = Mathf.Floor((rect.yMax + sizeOfJet * 8) / sizeOfJet) * sizeOfJet;

            float min = rect.yMin - sizeOfJet * 8;

            for (float y = start; y > min; y -= sizeOfJet)
            {
                value.Add(Mathf.Round(y * 10) / 10);
            }

            return value;
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= ResetCells;
        }
    }
}
