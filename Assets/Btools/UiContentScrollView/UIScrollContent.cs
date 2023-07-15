using UnityEngine;

namespace Btools.Components
{
    public class UIScrollContent : MonoBehaviour
    {
        private RectTransform _child;

        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                _rectTransform = _rectTransform != null ? _rectTransform : GetComponent<RectTransform>();
                return _rectTransform;
            }
            private set
            {
                _rectTransform = value;
            }
        }
        public RectTransform Child
        {
            get
            {
                _child ??= GetComponentInChildren<RectTransform>();
                return _child;
            }
            set
            {
                if (_child is null)
                    _child = value;
                else
                    throw new System.InvalidOperationException("Can only set child once.");
            }
        }

        public float Width => Child.sizeDelta.x;
        public float Height => Child.sizeDelta.y;

        public static UIScrollContent New(RectTransform parent, RectTransform child)
        {
            GameObject newObj = new GameObject("UI Scroll Content");
            UIScrollContent uIScrollContent = newObj.AddComponent<UIScrollContent>();
            newObj.transform.SetParent(parent);
            uIScrollContent.RectTransform = newObj.AddComponent<RectTransform>();

            uIScrollContent.RectTransform.localScale = Vector2.one;

            Vector3 oldChildScale = child.localScale;

            uIScrollContent.Child = child;
            child.SetParent(uIScrollContent.RectTransform);
            child.localPosition = Vector2.zero;
            child.localScale = oldChildScale;

            uIScrollContent.RectTransform.sizeDelta = child.sizeDelta;
            return uIScrollContent;
        }
    }
}
