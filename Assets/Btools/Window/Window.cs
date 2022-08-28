using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEditor;
using Btools.utils;

namespace Btools.Components
{
    /// <summary>Makes a window (as a template)</summary>
    public class Window : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Canvas canvas;

        [SerializeField]
        private TextMeshProUGUI Title;

        [SerializeField]
        private Image titlebar, CloseButton;

        [SerializeField, HideInInspector]
        private Color colour;

        [SerializeField]
        private bool customTheme;

        private bool _IsFullscren;

        public Color Colour
        {
            get => colour;

            set
            {
                colour = value;
                ResetColour();
            }
        }

        public bool CustomTheme
        {
            get => customTheme;
            set => customTheme = value;
        }
        public string TitleText
        {
            get => Title.text;
            set => Title.text = value;
        }

        public Rect WindowRect => rectTransform.rect;

        public bool IsOpen => gameObject.activeSelf;

        private Vector2 _StartScale;

        public bool IsFullscreen
        {
            get => _IsFullscren;
            set
            {
                _IsFullscren = value;
                if (value)
                {
                    rectTransform.sizeDelta = new Vector2(canvas.pixelRect.width / rectTransform.localScale.x, canvas.pixelRect.height / rectTransform.localScale.y) / canvas.scaleFactor;
                    rectTransform.anchoredPosition = Vector2.zero;
                    return;
                }

                rectTransform.sizeDelta = _StartScale;
            }
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            ResetColour();
            _StartScale = rectTransform.sizeDelta;
        }

        public void Fullscreen() =>
            IsFullscreen = !IsFullscreen;

        public void Dragging(PointerEventData eventData)
        {
            if (IsFullscreen)
                return;

            rectTransform.position += (Vector3)eventData.delta;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Open(string title)
        {
            Title.text = title;
            gameObject.SetActive(true);
        }

        public void Resize(PointerEventData eventData)
        {
            if (IsFullscreen)
                return;
            Vector2 movement = eventData.delta;
            movement.y *= -1;

            if (rectTransform.sizeDelta.x <= 0 && movement.x < 0)
            {
                movement.x = 0;
                rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
            }
            if (rectTransform.sizeDelta.y <= 0 && movement.y < 0)
            {
                movement.y = 0;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0);
            }

            rectTransform.sizeDelta += movement / canvas.scaleFactor / 2;
            rectTransform.anchoredPosition -= new Vector2(-movement.x, movement.y) / canvas.scaleFactor / 2;

        }

        public void ResetColour()
        {
            if (customTheme)
                return;
            Color.RGBToHSV(Colour, out float H, out float S, out float V);

            Color lightColour = Color.HSVToRGB(H, S - .25f, V);
            Color darkColour = Color.HSVToRGB(H, S - .15f, V - .5f);

            if (titlebar)
                titlebar.color = lightColour;

            Image myImg = GetComponent<Image>();

            if (myImg)
                myImg.color = darkColour;

            if (CloseButton)
                CloseButton.color = darkColour;

            if (Title)
                Title.color = InvertColour(Colour);
        }
        private Color InvertColour(Color c)
        {
            return new Color
            {
                r = 1f - c.r,
                g = 1f - c.g,
                b = 1f - c.b,
                a = c.a,
            };
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Window))]
    public class WindowEditor : Editor
    {
        public SerializedProperty Title;
        public SerializedProperty titlebar;
        public SerializedProperty CloseButton;
        public SerializedProperty colour;
        public SerializedProperty customTheme;

        void OnEnable()
        {
            Title = serializedObject.FindProperty("Title");
            titlebar = serializedObject.FindProperty("titlebar");
            CloseButton = serializedObject.FindProperty("CloseButton");
            colour = serializedObject.FindProperty("colour");
            customTheme = serializedObject.FindProperty("customTheme");
        }

        public override void OnInspectorGUI()
        {
            Window script = target as Window;
            TextMeshProUGUI title = (TextMeshProUGUI)Title.objectReferenceValue;

            EditorGUILayout.PropertyField(customTheme, new GUIContent("Use Custom Theme"));

            if (!customTheme.boolValue)
                EditorGUILayout.PropertyField(colour, new GUIContent("Colour"));

            EditorGUILayout.PropertyField(Title, new GUIContent("Title Component"));
            EditorGUILayout.PropertyField(titlebar, new GUIContent("Title bar"));
            EditorGUILayout.PropertyField(CloseButton, new GUIContent("CloseButton"));

            if (title)
            {
                string oldText = title.text;
                string newText = EditorGUILayout.TextField("Title", oldText);

                if (oldText != newText)
                    title.text = newText;
            }

            script.ResetColour();
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}