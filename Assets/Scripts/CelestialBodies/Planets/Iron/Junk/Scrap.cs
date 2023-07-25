using System;
using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies.Planets.Iron
{
    public class Scrap : CelestialBody
    {
        public override string TypeString => "Scrap";

        public override string TravelTarget => string.Empty;

        public override bool Circular => false;

        private ScrapType _scrap;
        public ScrapType ScrapData
        {
            get => _scrap;
            set
            {
                _scrap = value;
                OnScrapChange?.Invoke();
            }
        }
        public Action OnScrapChange;

        public override void Create(Vector2 pos)
        {
            Position = pos;
            var data = (ScrapType)RandomNum.Get(1, int.MaxValue, RandomNumberGenerator);
            data |= ScrapType.Sheet; // MUST have sheet
            ScrapData = data;
            Name = RandomNum.GetString(10, RandomNumberGenerator); // why not
            Width = RandomNum.GetFloat(.8f, 1.2f, RandomNumberGenerator) * Measurement.M;
            Height = Width;
        }

        public enum ScrapType
        {
            Sheet = 1,
            Pole = 2,
            Disc = 4,
            Lights = 8,
        }

        private void Set(ScrapType type, bool enabled)
        {
            if (enabled)
            {
                ScrapData |= type;
                return;
            }

            if ((ScrapData & type) == type)
                ScrapData -= type;
        }

        [InspectableVar("Sheet")]
        public bool Sheet { get => (ScrapData & ScrapType.Sheet) == ScrapType.Sheet; set => Set(ScrapType.Sheet, value); }

        [InspectableVar("Pole")]
        public bool Pole { get => (ScrapData & ScrapType.Pole) == ScrapType.Pole; set => Set(ScrapType.Pole, value); }
        [InspectableVar("Disc")]
        public bool Disc { get => (ScrapData & ScrapType.Disc) == ScrapType.Disc; set => Set(ScrapType.Disc, value); }
        [InspectableVar("Lights")]
        public bool Lights { get => (ScrapData & ScrapType.Lights) == ScrapType.Lights; set => Set(ScrapType.Lights, value); }
    }
}
