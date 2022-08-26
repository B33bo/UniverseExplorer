using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Btools.meta
{
#if UNITY_EDITOR
    /// <summary>Only used for the Gameobject/Create function for butil</summary>
    public class GameobjectMakers
    {
        /// <summary>Creates a UI element, as it would normally</summary>
        /// <param name="path">file Path of the prefab</param>
        public static void LoadUI(string path)
        {
            if (Selection.activeGameObject == null)
            {
                //Canvases should automatically be selected if nothing else is
                Canvas newCanvas = MonoBehaviour.FindObjectOfType<Canvas>();
                if (newCanvas != null)
                    Selection.activeTransform = newCanvas.transform;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            Selection.activeObject = PrefabUtility.InstantiatePrefab(prefab, Selection.activeTransform);

            PrefabUtility.UnpackPrefabInstance(Selection.activeGameObject, PrefabUnpackMode.OutermostRoot, InteractionMode.AutomatedAction);
        }

        [MenuItem("GameObject/UI/Slider2D - butil", priority = 2033)]
        public static void Slider2D() =>
            LoadUI("Assets/Btools/Slider2D/Slider2D.prefab");

        [MenuItem("GameObject/UI/DialougeBox - butil", priority = 3000)]
        public static void DialougeBox()
        {
            LoadUI("Assets/Btools/Dialouge/Dialouge.prefab");
        }

        [MenuItem("GameObject/UI/Joystick - butil", priority = 3000)]
        public static void Joystick()
        {
            LoadUI("Assets/Btools/Joystick/Joystick.prefab");
        }

        [MenuItem("GameObject/UI/Color Picker - butil", priority = 3000)]
        public static void ColorPicker()
        {
            LoadUI("Assets/Btools/ColorPicker/ColorPicker.prefab");
        }

        [MenuItem("GameObject/UI/Window - butil", priority = 3000)]
        public static void Window()
        {
            LoadUI("Assets/Btools/Window/Window.prefab");
        }

        [MenuItem("GameObject/UI/DevConsole - butil", priority = 3000)]
        public static void DevConsole()
        {
            LoadUI("Assets/Btools/DevConsole/DevConsole.prefab");
        }

        [MenuItem("GameObject/UI/multi Tool - butil", priority = 3000)]
        public static void UImultiTool()
        {
            LoadUI("Assets/Btools/UImultiTool/UImultitool.prefab");
        }

        [MenuItem("GameObject/UI/UiContent Scroll View - butil", priority = 3000)]
        public static void UiContentScrollView()
        {
            LoadUI("Assets/Btools/UImultiTool/UImultitool.prefab");
        }
    }
#endif

    /// <summary>Things related to the library itself</summary>
    public static class Btools
    {
        /// <summary>The current version of Butil</summary>
        public static float version = 4.10f;
    }
}

namespace Btools.Testing
{

}