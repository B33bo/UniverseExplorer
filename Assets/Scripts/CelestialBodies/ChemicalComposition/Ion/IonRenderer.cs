using UnityEngine;

namespace Universe.CelestialBodies.Atomic
{
    public class IonRenderer : CelestialBodyRenderer
    {
        [SerializeField]
        private ProtonRenderer protonRenderer;

        [SerializeField]
        private NeutronRenderer neutronRenderer;

        [SerializeField]
        private ElectronRenderer electronRenderer;

        public static string Symbol;

        [SerializeField]
        private Transform ring;

        private Ion ion;

        private void Start()
        {
            CameraControl.Instance.transform.position = Vector3.zero;
            Spawn(Vector2.zero, 0, (BodyManager.Parent as IonObject).ion);
        }

        private void LoadIon()
        {
            ion = (Target as IonObject).ion;
            LoadNucleus(out float length);
            LoadElectrons(length);
        }

        private bool SpawnProton(int index, ref int remainingNeutrons, ref int remainingProtons)
        {
            if (index % 2 == 0)
            {
                if (remainingProtons > 0)
                {
                    remainingProtons--;
                    return true;
                }

                remainingNeutrons--;
                return false;
            }

            if (remainingNeutrons > 0)
            {
                remainingNeutrons--;
                return false;
            }

            remainingProtons--;
            return true;
        }

        private void LoadNucleus(out float length)
        {
            int nucleusItems = ion.NumberofNeutrons + ion.NumberofProtons;
            bool spawnOnlyElectrons = false;

            if (nucleusItems == 0)
            {
                spawnOnlyElectrons = true;
                nucleusItems = ion.NumberofElectrons;
            }

            float currentRotation = 0;
            length = 0;
            float tilesPerRing = 4;

            int remainingNeutrons = ion.NumberofNeutrons;
            int remainingProtons = ion.NumberofProtons;

            for (int i = 0; i < nucleusItems; i++)
            {
                CelestialBodyRenderer newItemToSpawn;

                if (spawnOnlyElectrons)
                    newItemToSpawn = electronRenderer;
                else if (SpawnProton(i, ref remainingNeutrons, ref remainingProtons))
                    newItemToSpawn = protonRenderer;
                else
                    newItemToSpawn = neutronRenderer;

                var newItem = Instantiate(newItemToSpawn, transform);

                Vector2 positionOfCurrent = new Vector2(Mathf.Cos(currentRotation), Mathf.Sin(currentRotation)) * length;
                newItem.Spawn(positionOfCurrent, Target.Seed + i);

                currentRotation += (2 * Mathf.PI) / tilesPerRing;

                //-.1 to approximate
                if (currentRotation >= (2 * Mathf.PI - .1f) || i == 0)
                {
                    currentRotation = 0;
                    length += 1;
                    tilesPerRing *= 2;
                }
            }
        }

        private void LoadElectrons(float length)
        {
            if (ion.NumberofProtons + ion.NumberofNeutrons == 0)
                return;
            int lastShellNumber = -1;
            int electronsInOuterShell = (ion.NumberofElectrons - 2) % 8;
            for (int i = 0; i < ion.NumberofElectrons; i++)
            {
                float electronsInShell = 2;
                int shellNumber;
                int indexInShell = i < 2 ? i : (i - 2) % 8;

                if (i < 2)
                    shellNumber = 0;
                else
                    shellNumber = (i - 2) / 8 + 1;

                if (lastShellNumber != shellNumber)
                {
                    var newRing = Instantiate(ring, transform);
                    newRing.transform.localScale = ((shellNumber + 1) * 5 + length) * 2 * Vector2.one;
                }

                if (shellNumber > 0)
                {
                    if (ion.NumberofElectrons - electronsInOuterShell > i)
                        electronsInShell = 8;
                    else
                        electronsInShell = electronsInOuterShell;
                }

                var newElectron = Instantiate(electronRenderer, transform);

                float currentRotation = (indexInShell / electronsInShell) * 360;

                Debug.Log((indexInShell + 1) + " / " + (float)electronsInShell);
                newElectron.InitOrbiter((shellNumber + 1) * 5 + length, currentRotation);
                newElectron.Spawn(new Vector2(1, 1), Target.Seed + i);

                lastShellNumber = shellNumber;
            }
        }

        public override void Spawn(Vector2 pos, int? seed)
        {
            Target = new IonObject();
            if (seed.HasValue)
                Target.SetSeed(seed.Value);
            Target.Create(pos);

            (Target as IonObject).IsAbstract = false;
            LoadIon();
        }

        public void Spawn(Vector2 pos, int? seed, Ion ion)
        {
            IonObject ionObject = new IonObject();
            Target = ionObject;
            if (seed.HasValue)
                ionObject.SetSeed(seed.Value);
            ionObject.Create(pos, ion);
            ionObject.IsAbstract = false;
            LoadIon();
        }
    }
}
