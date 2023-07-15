using System.Collections;
using UnityEngine;
using Universe.CelestialBodies.Planets;

namespace Universe
{
    public class StarColorer : ColorHighlights
    {
        protected override void OnAwake()
        {
            if (!(BodyManager.Parent is Star star))
                return;

            primary = star.starColor;
        }

        private IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            CameraControl.Instance.MyCamera.backgroundColor = primary;
        }
    }
}
