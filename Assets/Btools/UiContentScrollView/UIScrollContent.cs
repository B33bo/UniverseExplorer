using UnityEngine;

namespace Btools.Components
{
    public class UIScrollContent : MonoBehaviour
    {
        private RectTransform _child;
        public RectTransform rectTransform { get; private set; }
        public RectTransform child
        {
            get => _child;
            set
            {
                if (_child is null)
                    _child = value;
                else
                    throw new System.InvalidOperationException("Can only set child once.");
            }
        }

        public float Width => child.sizeDelta.x;
        public float Height => child.sizeDelta.y;

        public static UIScrollContent New(RectTransform parent, RectTransform child)
        {
            GameObject newObj = new GameObject("UI Scroll Content");
            UIScrollContent uIScrollContent = newObj.AddComponent<UIScrollContent>();
            newObj.transform.SetParent(parent);
            uIScrollContent.rectTransform = newObj.AddComponent<RectTransform>();

            uIScrollContent.rectTransform.localScale = Vector2.one;

            Vector3 oldChildScale = child.localScale;

            uIScrollContent.child = child;
            child.SetParent(uIScrollContent.rectTransform);
            child.localPosition = Vector2.zero;
            child.localScale = oldChildScale;

            uIScrollContent.rectTransform.sizeDelta = child.sizeDelta;
            return uIScrollContent;
        }
    }
}
