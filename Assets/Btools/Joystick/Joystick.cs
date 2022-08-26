using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Btools.Components
{
    /// <summary>Joysticks for mobile use</summary>
    public class Joystick : MonoBehaviour, IDragHandler
    {
        [SerializeField]
        private float radius;
        private RectTransform rt;
        private Canvas canvas;
        public RectTransform handle;
        [Space]
        public UnityEvent<Vector2> OnValueChanged = new UnityEvent<Vector2>();

        /// <summary>The radius of the joystick</summary>
        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;

                if (!rt)
                    rt = GetComponent<RectTransform>();

                if (!canvas)
                    canvas = transform.GetComponentInParent<Canvas>();

                if (!rt || !canvas)
                    return;

                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, radius * 2);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, radius * 2);

                Vector3 difference = Input.mousePosition - rt.position;
                handle.localPosition = Vector3.ClampMagnitude(difference / canvas.scaleFactor, radius);
            }
        }

        /// <summary>
        /// The value of the Joystick. If it's in the top right, it returns root(2) since it's a circle pattern
        /// This is useful for movement, since you move in a circle and you don't need to normalise any vectors
        /// </summary>
        public Vector2 Value
        {
            get
            {
                float x = handle.localPosition.x / rt.rect.width;
                float y = handle.localPosition.y / rt.rect.height;

                return new Vector2(x, y) * 2;
            }
            set
            {
                Rect rect = rt.rect;
                float x = Mathf.Lerp(rect.xMin, rect.xMax, value.x);
                float y = Mathf.Lerp(rect.yMin, rect.yMax, value.y);

                handle.localPosition = new Vector2(x, y);
            }
        }

        void OnValidate()
        {
            Radius = radius;
        }

        void Awake()
        {
            rt = GetComponent<RectTransform>();
            canvas = transform.GetComponentInParent<Canvas>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            //'clamp magnitude' clamps the magnitude, but allows the other axis to go freely, resulting in a circle
            //This can be problematic because it would always be in the bottom-left cornet,
            //which is why we need "handle.localPosition" instead of "handle.position"

            Vector3 difference = Input.mousePosition - rt.position;
            handle.localPosition = Vector3.ClampMagnitude(difference / canvas.scaleFactor, radius);

            OnValueChanged.Invoke(Value);
        }
    }
}
