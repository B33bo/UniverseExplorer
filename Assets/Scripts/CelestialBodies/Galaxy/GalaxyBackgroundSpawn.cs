using System.Collections.Generic;
using UnityEngine;
using Universe.CelestialBodies;

namespace Universe
{
    public class GalaxyBackgroundSpawn : Spawner
    {
        [SerializeField]
        private float maximumCamera;

        public override void OnStart()
        {
            base.OnStart();
            CameraControl.Instance.OnFinishedLoading += SetCameraCol;
        }

        private void SetCameraCol()
        {
            Color a = Color.red;
            Color b = Color.blue;

            if (BodyManager.Parent is SpiralGalaxy spiralGalaxy)
            {
                a = spiralGalaxy.outer;
                b = spiralGalaxy.inner;
            }

            a *= .2f;
            b *= .2f;

            CameraControl.Instance.MyCamera.backgroundColor = (a + b) / 2f;
            CameraControl.Instance.OnFinishedLoading -= SetCameraCol;
        }

        public override List<Vector2> CellsOnScreen(Rect cameraBounds)
        {
            if (CameraControl.Instance.MyCamera.orthographicSize > maximumCamera)
                return new List<Vector2>(); // more efficient
            return base.CellsOnScreen(cameraBounds);
        }
    }
}
