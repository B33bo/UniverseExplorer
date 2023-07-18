using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Universe.Inspector
{
    public class ObjectInspector : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        [SerializeField]
        private RectTransform scrollContent;

        private void Start()
        {
            Type t = ObjectDataLoader.celestialBody.GetType();

            float pos = 0;
            foreach (var prop in t.GetProperties())
            {
                var attribute = prop.GetCustomAttribute<InspectableVarAttribute>();
                if (attribute == null)
                    continue;

                string uiName = prop.PropertyType.IsEnum ? GetUiType[typeof(Enum)] : GetUiType[prop.PropertyType];

                var variablePrefab = Resources.Load<VariableUI>(uiName);
                float halfHeight = variablePrefab.Height / 2;
                pos -= halfHeight;
                var newVariable = Instantiate(variablePrefab, scrollContent);
                newVariable.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, pos);
                newVariable.Init(new Variable(attribute.VariableName, () => prop.GetValue(ObjectDataLoader.celestialBody),
                    x => prop.SetValue(ObjectDataLoader.celestialBody, x)), attribute.Params);
                pos -= halfHeight;
            }

            scrollContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, -pos);
        }
    
        public void Close()
        {
            animator.Play("Unfocus");
            Btools.TimedEvents.Timed.RunAfterTime(() =>
            {
                if (SceneManager.GetSceneByName("Inspector").isLoaded)
                    SceneManager.UnloadSceneAsync("Inspector");
            }, .5f);
            CameraControl.Instance.UnFocus();
        }

        private static readonly Dictionary<Type, string> GetUiType = new Dictionary<Type, string>()
        {
            { typeof(string), "Inspector/String" },
            { typeof(Color), "Inspector/Color" },
            { typeof(ColorHSV), "Inspector/Color" },
            { typeof(bool), "Inspector/Boolean" },
            { typeof(float), "Inspector/Number" },
            { typeof(double), "Inspector/Number" },
            { typeof(int), "Inspector/Number" },
            { typeof(Enum), "Inspector/Dropdown" },
        };
    }
}
