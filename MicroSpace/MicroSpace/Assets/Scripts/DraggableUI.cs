using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class DraggableUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 _dragOffset;
        public GameObject GameObjectToMove;
        private bool _dragStarted = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            var worldCoord = Camera.main.ScreenToWorldPoint(eventData.position);
            _dragOffset = GameObjectToMove.transform.position - worldCoord;
            _dragStarted = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!_dragStarted) return;
            var worldCoord = Camera.main.ScreenToWorldPoint(eventData.position);
            var newPos = new Vector3(worldCoord.x + _dragOffset.x, worldCoord.y + _dragOffset.y, GameObjectToMove.transform.position.z);
            GameObjectToMove.transform.position = newPos;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var pos = Camera.main.WorldToViewportPoint(GameObjectToMove.transform.position);
            pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
            pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
            _dragStarted = false;
        }
    }
}
