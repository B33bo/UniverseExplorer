using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace Btools.Components
{
    public class UImultiTool : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI NameText;

        [SerializeField]
        Slider SliderInput;

        [SerializeField]
        TMP_InputField TextInput;

        [SerializeField]
        float value;

        [SerializeField, Space]
        UnityEvent<float> onValueChanged;

        public string Name { get => NameText.text; set => NameText.text = value; }
        public float Value
        {
            get => value; set
            {
                onValueChanged.Invoke(value);
                this.value = value;

                if (SliderInput)
                    SliderInput.value = value;

                if (TextInput)
                    TextInput.text = value.ToString();
            }
        }

        void OnValidate()
        {
            if (SliderInput)
                SliderInput.value = value;

            if (TextInput)
                TextInput.text = value.ToString();
        }

        public void SetValue(string value)
        {
            if (!float.TryParse(value, out float result))
                return;

            Value = result;
        }
    }
}
