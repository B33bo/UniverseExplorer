using UnityEngine;

namespace Universe
{
    public class ObjectDataLoader : MonoBehaviour
    {
        public static CelestialBody celestialBody;
        [SerializeField, Multiline]
        private string FormattedString;

        [SerializeField]
        private TMPro.TextMeshProUGUI nameTextComponent, extraInfoTextComponent;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private GameObject travelButton;

        public static ObjectDataLoader Instance { get; private set; }

        private void Awake()
        {
            if (celestialBody is null)
                celestialBody = new CelestialBodies.UnknownItem();
            Instance = this;
            extraInfoTextComponent.text = string.Format(FormattedString,
                celestialBody.Name,
                celestialBody.TypeString,
                $"{celestialBody.Position.x}, {celestialBody.Position.y}",
                GetRadiusString(celestialBody.Circular),
                celestialBody.Mass.ToCleanString(),
                celestialBody.Seed) + "\n" + celestialBody.GetBonusTypes();

            nameTextComponent.text = celestialBody.Name;

            if (celestialBody.TravelTarget == string.Empty)
                Destroy(travelButton);
        }

        private string GetRadiusString(bool Circular)
        {
            if (Circular)
            {
                var radius = celestialBody.Width / 2;
                DistanceUnit distanceUnit = UnitConversion.ReasonableFormat(radius);
                return "Radius - " +
                    UnitConversion.Convert(radius, DistanceUnit.Kilometers, distanceUnit).ToCleanString() + " " + UnitConversion.ToAbbreviation(distanceUnit);
            }

            DistanceUnit unit = UnitConversion.ReasonableFormat((celestialBody.Width + celestialBody.Height) / 2);
            string abbr = UnitConversion.ToAbbreviation(unit); //haha, abbr is an abbreviation for 'abbreviation'

            string width = System.Math.Round(UnitConversion.Convert(celestialBody.Width, DistanceUnit.Kilometers, unit)).ToCleanString();
            string height = System.Math.Round(UnitConversion.Convert(celestialBody.Height, DistanceUnit.Kilometers, unit)).ToCleanString();

            return "Scale - " + width + "x" + height + " " + abbr;
        }

        public void Unfocus()
        {
            animator.Play("Unfocus");
            Btools.TimedEvents.Timed.RunAfterTime(() => { UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("ObjectData"); }, .5f);
            CameraControl.Instance.UnFocus();
        }

        public void Travel()
        {
            BodyManager.TravelTo(celestialBody.TravelTarget);
            BodyManager.Parent = celestialBody;
        }
    }
}
