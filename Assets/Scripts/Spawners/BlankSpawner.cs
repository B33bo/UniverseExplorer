using UnityEngine;
using UnityEditor;

namespace Universe
{
    public class BlankSpawner : Spawner
    {
        public override void OnStart() { }
        public override void ReloadCells(Rect cameraRect) { }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(BlankSpawner))]
    public class BlankSpawnerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((BlankSpawner)target), typeof(BlankSpawner), false);
            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}
