using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Btools.Components
{
    public class UIContentScrollView : MonoBehaviour, IList<UIScrollContent>
    {
        [SerializeField]
        private RectTransform Content;

        [SerializeField]
        private List<UIScrollContent> items = new List<UIScrollContent>();

        private UnityEngine.UI.ScrollRect scrollRect;
        private bool ReloadNextFrame;

        public UIScrollContent this[int index]
        {
            get => items[index];
            set
            {
                items[index] = value;
                ReadjustList();
            }
        }

        public bool IsReadOnly => false;

        public int Count => items.Count;

        [SerializeField]
        private float _Padding;
        public float Padding
        {
            get => _Padding;
            set
            {
                _Padding = value;
                ReadjustList();
            }
        }

        [SerializeField]
        private Direction _Orientation = Direction.TopToBottom;
        public Direction Orientation
        {
            get => _Orientation;
            set
            {
                _Orientation = value;
                ReadjustList();
            }
        }

        public enum Direction : byte
        {
            LeftToRight,
            TopToBottom,
        }

        private void Awake()
        {
            scrollRect = GetComponent<UnityEngine.UI.ScrollRect>();
        }

        private void Update()
        {
            if (!ReloadNextFrame)
                return;
            ReloadNextFrame = false;
            ForceReloadList();
        }

        public void Add(RectTransform rectTransform)
        {
            UIScrollContent newUiScrollContent = UIScrollContent.New(parent: Content, child: rectTransform);
            items.Add(newUiScrollContent);
            ReadjustList();
        }

        public void Add(UIScrollContent uIScrollContent)
        {
            items.Add(uIScrollContent);
            ReadjustList();
        }

        public void Clear()
        {
            for (int i = 0; i < items.Count; i++)
                Destroy(items[i].gameObject);
            items.Clear();
            ReadjustList();
        }

        public bool Contains(UIScrollContent item) =>
            items.Contains(item);

        public void CopyTo(UIScrollContent[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<UIScrollContent> GetEnumerator() =>
            new Enumerator(this);

        public int IndexOf(UIScrollContent item) =>
            items.IndexOf(item);

        public void Insert(int index, UIScrollContent item)
        {
            items.Insert(index, item);
            ReadjustList();
        }

        public bool Remove(UIScrollContent item)
        {
            bool returnValue = items.Remove(item);
            Destroy(item.gameObject);
            ReadjustList();
            return returnValue;
        }

        public void RemoveAt(int index)
        {
            Destroy(items[index].gameObject);
            items.RemoveAt(index);
            ReadjustList();
        }

        public void ReadjustList() =>
            ReloadNextFrame = true;

        public void ForceReloadList()
        {
            if (scrollRect is null)
                scrollRect = GetComponent<UnityEngine.UI.ScrollRect>();

            if (scrollRect is null)
                return;

            switch (Orientation)
            {
                case Direction.LeftToRight:
                    scrollRect.horizontal = true;
                    scrollRect.vertical = false;
                    Content.anchorMin = new Vector2(0, 0);
                    Content.anchorMax = new Vector2(0, 1);
                    Content.anchoredPosition = Vector2.zero;
                    break;
                case Direction.TopToBottom:
                    scrollRect.horizontal = false;
                    scrollRect.vertical = true;
                    Content.anchorMin = new Vector2(0, 1);
                    Content.anchorMax = new Vector2(1, 1);
                    Content.anchoredPosition = Vector2.zero;
                    break;
                default:
                    Debug.LogError($"{Orientation} is not a valid orientation");
                    scrollRect.horizontal = false;
                    scrollRect.vertical = false;
                    break;
            }

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i])
                    continue;
                Destroy(items[i]);
                items.RemoveAt(i);
                i--;
            }

            if (items.Count <= 0)
                return;

            float CurrentPosition = Padding;
            CurrentPosition += Orientation switch
            {
                Direction.LeftToRight => items[0].Width / 2,
                Direction.TopToBottom => items[0].Height / 2,
                _ => 0,
            };

            for (int i = 0; i < items.Count; i++)
            {
                switch (Orientation)
                {
                    case Direction.LeftToRight:
                        items[i].rectTransform.anchoredPosition = new Vector3(CurrentPosition, 0);
                        CurrentPosition += items[i].Width;

                        //Scale Left
                        items[i].rectTransform.anchorMin = new Vector2(0, 0);
                        items[i].rectTransform.anchorMax = new Vector2(0, 1);
                        break;
                    case Direction.TopToBottom:
                        items[i].rectTransform.anchoredPosition = new Vector3(0, -CurrentPosition);
                        CurrentPosition += items[i].Height;

                        //Scale Top
                        items[i].rectTransform.anchorMin = new Vector2(0, 1);
                        items[i].rectTransform.anchorMax = new Vector2(1, 1);
                        break;
                    default:
                        items[i].rectTransform.position = new Vector3(0, 0);
                        break;
                }
                CurrentPosition += Padding;

                if (i == items.Count - 1)
                    continue;
            }

            switch (Orientation)
            {
                case Direction.LeftToRight:
                    Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CurrentPosition);
                    Content.offsetMin = new Vector2(Content.offsetMin.x, 0);
                    Content.offsetMax = new Vector2(Content.offsetMax.x, 0);
                    break;
                case Direction.TopToBottom:
                    Content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CurrentPosition);
                    Content.offsetMin = new Vector2(0, Content.offsetMin.y);
                    Content.offsetMax = new Vector2(0, Content.offsetMax.y);
                    break;
                default:
                    break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() =>
            new Enumerator(this);

        public class Enumerator : IEnumerator<UIScrollContent>
        {
            public UIContentScrollView uiContentScrollView;
            private int Position = -1;

            public UIScrollContent Current
            {
                get
                {
                    if (Position >= uiContentScrollView.items.Count || Position < 0)
                        throw new InvalidOperationException();

                    return uiContentScrollView.items[Position];
                }
            }

            object IEnumerator.Current => Current;

            public Enumerator(UIContentScrollView baseUIcontentScrollView) =>
                uiContentScrollView = baseUIcontentScrollView;

            public bool MoveNext()
            {
                Position++;
                return Position < uiContentScrollView.items.Count;
            }

            public void Reset()
            {
                Position = -1;
            }

            public void Dispose()
            {

            }
        }

    }
#if UNITY_EDITOR
    [CustomEditor(typeof(UIContentScrollView))]
    public class UIContentScrollViewEditor : Editor
    {
        private SerializedProperty Content;
        private SerializedProperty Items;

        private void OnEnable()
        {
            Content = serializedObject.FindProperty("Content");
            Items = serializedObject.FindProperty("items");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UIContentScrollView script = (UIContentScrollView)target;
            EditorGUILayout.PropertyField(Content);

            float newPadding = EditorGUILayout.FloatField("Distance between objects", script.Padding);

            if (newPadding != script.Padding)
                script.Padding = newPadding;

            var NewOrientation = (UIContentScrollView.Direction)EditorGUILayout.EnumPopup("Orientation", script.Orientation);
            if (NewOrientation != script.Orientation)
                script.Orientation = NewOrientation;

            int oldCount = script.Count;
            EditorGUILayout.PropertyField(Items);
            if (oldCount != script.Count)
                script.ReadjustList();

            if (GUILayout.Button("Refresh"))
                script.ReadjustList();

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
