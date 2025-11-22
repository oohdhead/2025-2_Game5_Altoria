using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUI
{
    public class UIDragRotateArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("Target Model")]
        [SerializeField] Transform target;

        [Header("Drag Area")]
        [SerializeField] RectTransform dragArea;

        [Header("Rotation Speed")]
        [SerializeField] float rotationSpeed = 0.3f;

        bool isDraggingValid = false;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (dragArea == null)
                return;

            isDraggingValid = RectTransformUtility.RectangleContainsScreenPoint(
                dragArea,
                eventData.position,
                eventData.pressEventCamera
            );
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isDraggingValid) return;
            if (target == null) return;

            float deltaX = eventData.delta.x;

            float angle = deltaX * rotationSpeed * -1;

            target.Rotate(0f, angle, 0f, Space.World);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDraggingValid = false;
        }
    }
}