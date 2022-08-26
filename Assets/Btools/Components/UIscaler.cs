using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Btools.Components
{
    /// <summary>Scales an image to the right dimenstions in the UI</summary>
    [AddComponentMenu("UI/Image Scaler")]
    [System.Serializable]
    public class UIscaler : MonoBehaviour
    {
        [SerializeField]
        private bool useXAxis;

        public bool UseXAxis
        {
            get => useXAxis;
            set
            {
                useXAxis = value;
                ResetSprite();
            }
        }

        [SerializeField]
        private float scale;

        /// <summary>The scale of the UI item</summary>
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                ResetSprite();
            }
        }

        [SerializeField]
        private RectTransform rectTransform;

        [SerializeField]
        private Image sp;

        [SerializeField]
        private PositionType imagePositionType;

        [SerializeField]
        private Vector2 anchorPositionOffset;

        [SerializeField]
        private Sprite oldSprite;

        public Vector2 Offset
        {
            get => anchorPositionOffset;
            set
            {
                anchorPositionOffset = value;
                ResetSprite();
            }
        }

        public PositionType ImagePositionType
        {
            get => imagePositionType;
            set
            {
                imagePositionType = value;
                ResetSprite();
            }
        }

        public enum PositionType
        {
            None,
            PositionByAnchor,
            PositionByAnchorAndScale,
        }

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            sp = GetComponent<Image>();
        }

        /// <summary>Forces it to retry</summary>
        public void ResetSprite()
        {
            if (!rectTransform)
                rectTransform = GetComponent<RectTransform>();

            if (!sp)
                sp = GetComponent<Image>();

            if (!sp.sprite)
            {
                rectTransform.sizeDelta = new Vector2(Scale, Scale);
                if (imagePositionType == PositionType.PositionByAnchor)
                    rectTransform.anchoredPosition = Offset;
                else if (imagePositionType == PositionType.PositionByAnchorAndScale)
                    rectTransform.anchoredPosition = Offset * rectTransform.sizeDelta;
                return;
            }

            float width = sp.sprite.rect.width;
            float height = sp.sprite.rect.height;

            float aspectRatio;

            if (UseXAxis)
                aspectRatio = height / width;
            else
                aspectRatio = width / height;

            Vector2 size;
            if (UseXAxis)
                size = new Vector2(scale, scale * aspectRatio);
            else
                size = new Vector2(scale * aspectRatio, scale);
            rectTransform.sizeDelta = size;

            if (imagePositionType == PositionType.PositionByAnchor)
                rectTransform.anchoredPosition = Offset;
            else if (imagePositionType == PositionType.PositionByAnchorAndScale)
                rectTransform.anchoredPosition = Offset * size;
        }

        void Update()
        {
            if (sp.sprite == oldSprite)
                return;

            oldSprite = sp.sprite;
            ResetSprite();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIscaler))]
    public class UIscalerEditor : Editor
    {
        UIscaler script;

        private void OnEnable()
        {
            script = (UIscaler)target;
        }

        public override void OnInspectorGUI()
        {
            float scale = script.Scale;
            bool UseXAxis = script.UseXAxis;

            script.Scale = EditorGUILayout.FloatField("Scale", scale);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Axis selected");

            if (GUILayout.Button(UseXAxis ? "X axis" : "Y axis"))
                script.UseXAxis = !script.UseXAxis;

            EditorGUILayout.EndHorizontal();

            script.ImagePositionType = (UIscaler.PositionType)EditorGUILayout.EnumPopup("Position Type", script.ImagePositionType);

            if (script.ImagePositionType != UIscaler.PositionType.None)
            {
                script.Offset = EditorGUILayout.Vector2Field("Offset", script.Offset);
            }

            if (GUILayout.Button("Refresh"))
                script.ResetSprite();

            serializedObject.ApplyModifiedPropertiesWithoutUndo();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
            }
        }
    }
#endif
}
