using Btools.Components;
using UnityEditor;

namespace Btools.Editors
{
#if UNITY_EDITOR
    [CustomEditor(typeof(ColorPicker))]
    public class ColorPickerEditor : Editor
    {
        private SerializedProperty OnValueChanged;
        private void OnEnable()
        {
            OnValueChanged = serializedObject.FindProperty("OnValueChanged");
        }
        public override void OnInspectorGUI()
        {
            ColorPicker script = target as ColorPicker;
            script.ColorSpace = (ColorPicker.Type)EditorGUILayout.EnumPopup("Color Space", script.ColorSpace);
            EditorGUI.BeginChangeCheck();

            var color = EditorGUILayout.ColorField("Color", script.Color);

            if (EditorGUI.EndChangeCheck())
                script.Color = color;

            DrawDefaultInspector();
        }
    }
#endif
}
