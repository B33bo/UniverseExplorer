using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Btools.Components
{
    /// <summary>Simple draggable script</summary>
    [AddComponentMenu("UI/Draggable")]
    public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerDownHandler
    {
        public UnityEvent BeginDrag = new UnityEvent();
        public UnityEvent<PointerEventData> Drag = new UnityEvent<PointerEventData>();
        public UnityEvent EndDrag = new UnityEvent();
        public UnityEvent Clicked = new UnityEvent();

        public void OnBeginDrag(PointerEventData eventData)
        {
            BeginDrag.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            EndDrag.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Clicked.Invoke();
        }
    }
}
