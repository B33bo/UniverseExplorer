using UnityEngine;

namespace Universe
{
    public abstract class CelestialBody
    {
        private (double x, double y) _Scale;
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

        public int GetSeed()
        {
            if (BodyManager.Parent is null)
                return Position.GetHashCode();
                //return (int)(Position.x * Position.x + Position.y * 1053);
            return Position.GetHashCode() + BodyManager.GetSeed();
            //return (int)Mathf.Pow((int)Position.x, 2) + (int)Position.y + BodyManager.Parent.Seed;
        }

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
