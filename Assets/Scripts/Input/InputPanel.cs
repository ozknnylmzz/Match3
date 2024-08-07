using Match3.Enums;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3.Input
{
    public class InputPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            Vector2 worldPos = GetWorldPosition(eventData.position);

            EventBus.Instance.Publish<Vector2>(BoardEvents.OnPointerDown, worldPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                return;

            Vector2 worldPos = GetWorldPosition(eventData.position);

            EventBus.Instance.Publish<Vector2>(BoardEvents.OnPointerUp, worldPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
                return;

            Vector2 worldPos = GetWorldPosition(eventData.position);

            EventBus.Instance.Publish<Vector2>(BoardEvents.OnPointerDrag, worldPos);
        }

        private Vector2 GetWorldPosition(Vector2 screenPosition)
        {
            return Camera.main.ScreenToWorldPoint(screenPosition);
        }
    }
}