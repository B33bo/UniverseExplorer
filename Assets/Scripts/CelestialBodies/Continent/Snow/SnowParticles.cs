using UnityEngine;

namespace Universe.CelestialBodies.Biomes.Snow
{
    public class SnowParticles : MonoBehaviour
    {
        private System.Collections.IEnumerator Start()
        {
            var p = GetComponent<ParticleSystem>();
            if (BodyManager.Parent is null)
                p.randomSeed = 0;
            else
                p.randomSeed = (uint)BodyManager.Parent.Seed;

            p.Play();

            yield return new WaitForFrames(2); 
            CameraControl.Instance.OnPositionUpdate += CamUpdate;
        }

        private void OnDestroy()
        {
            CameraControl.Instance.OnPositionUpdate -= CamUpdate;
        }

        private void CamUpdate(Rect rect)
        {
            transform.position = new Vector3(rect.xMax, rect.yMax);
        }
    }
}
