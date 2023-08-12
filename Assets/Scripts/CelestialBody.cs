using UnityEngine;
using Universe.Inspector;

namespace Universe
{
    public abstract class CelestialBody
    {
        private (double x, double y) _Scale;

        [InspectableVar("Name")]
        public string Name { get; set; }
        public double Width { get => _Scale.x; set => _Scale.x = value; }
        public double Height { get => _Scale.y; set => _Scale.y = value; }
        public double Radius
        {
            get => _Scale.y / 2;
            set
            {
                value *= 2;
                _Scale.x = value;
                _Scale.y = value;
            }
        }
        public abstract string TypeString { get; }
        public abstract string TravelTarget { get; }
        public double Mass { get; set; }
        public abstract bool Circular { get; }

        public int Seed;

        public ChemicalComposition composition;
        public delegate void OnInspectedHandler(Variable val);
        public event OnInspectedHandler OnInspected;

        public int GetSeed()
        {
            if (BodyManager.Parent is null)
                return Position.GetHashCode();
            return Position.GetHashCode() + BodyManager.GetSeed();
        }

        public void Inspected(Variable val) => OnInspected?.Invoke(val);

        public void SetSeed(int seed)
        {
            Seed = seed;
            _randomNumberGenerator = new System.Random(seed);
        }

        public virtual string GetBonusTypes() => string.Empty;

        public Vector3 Position { get; set; }

        private System.Random _randomNumberGenerator = null;
        public System.Random RandomNumberGenerator
        {
            get
            {
                if (_randomNumberGenerator is null)
                {
                    Seed = GetSeed();
                    _randomNumberGenerator = new System.Random(Seed);
                }
                return _randomNumberGenerator;
            }
        }

        public CelestialBody(Vector3 position) { Position = position; Create(position); }
        public CelestialBody(Vector3 position, int Seed)
        {
            Position = position;
            _randomNumberGenerator = new System.Random(Seed);
            Create(position);
        }
        public CelestialBody() { }

        public abstract void Create(Vector2 pos);

        public override string ToString()
        {
            return $"{Name}\n{TypeString}\n{Position}\n{Width}x{Height}\nseed={Seed}";
        }
    }
}
