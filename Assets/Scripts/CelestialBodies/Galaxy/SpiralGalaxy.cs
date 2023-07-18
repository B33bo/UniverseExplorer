using System;
using UnityEngine;
using Universe.Inspector;

namespace Universe.CelestialBodies
{
    public class SpiralGalaxy : CelestialBody
    {
        public const double MinWidth = 10_000 * Measurement.LY, MaxWidth = 100_000 * Measurement.LY;
        public const double MinMass = 4e40 * Measurement.Kg, MaxMass = 4e42 * Measurement.Kg;
        public (Color outer, Color inner) Color;
        public bool IsRainbow;
        public Action OnRefreshColor;

        [InspectableVar("Outer")]
        public Color OuterEditable
        {
            get => Color.outer;
            set
            {
                Color.outer = value;
                OnRefreshColor.Invoke();
            }
        }

        [InspectableVar("Inner")]
        public Color InnerEditable
        {
            get => Color.inner;
            set
            {
                Color.inner = value;
                OnRefreshColor.Invoke();
            }
        }

        [InspectableVar("Rainbow")]
        public bool Rainbow
        {
            get => IsRainbow;
            set
            {
                IsRainbow = value;
                OnRefreshColor.Invoke();
            }
        }

        public override void Create(Vector2 position)
        {
            Position = position;

            Name = RandomNum.GetString(3, RandomNumberGenerator) + " " + RandomNum.Get(0, 1000, RandomNumberGenerator).ToString().PadLeft(3, '0');
            Radius = RandomNum.Get(MinWidth, MaxWidth, RandomNumberGenerator);
            Mass = RandomNum.Get(MinMass, MaxMass, RandomNumberGenerator);
        }

        public override string TypeString => "Spiral Galaxy";

        public override string TravelTarget => "Galaxy";

        public override bool Circular => true;

        public override string GetBonusTypes()
        {
            if (IsRainbow)
                return "Color - <#FF0000>R</color><#FFFF00>a</color><#00FF00>i</color><#00FFFF>n</color><#0000FF>b</color><#FF00FF>o</color><#A500FF>w</color>";
            return $"Color - <#{ColorUtility.ToHtmlStringRGB(Color.outer)}>{Color.outer.ToHumanString()}</color>";
        }
    }
}
