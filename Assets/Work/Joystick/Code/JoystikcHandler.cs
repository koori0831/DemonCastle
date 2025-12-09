using UnityEngine;
using UnityEngine.EventSystems;

namespace Work.Joystick.Code
{
    public class JoystikcHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public bool IsTouching { get; private set; } = false;
        [SerializeField] private RectTransform joystickKnob;
        [SerializeField] private RectTransform joystickParantImage;
        [SerializeField] private float joystickRange = 50f;

        public delegate void JoystickEvent(Vector3 prev, Vector3 current);
        public event JoystickEvent OnMoveDirectionChangedEvent;
        private Vector2 _startPos;

        private Vector3 _movePos;

        public void OnPointerUp(PointerEventData eventData)
        {
            IsTouching = false;
            joystickParantImage.gameObject.SetActive(false);
            OnMoveDirectionChangedEvent?.Invoke(_movePos, Vector3.zero);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsTouching = true;
            joystickParantImage.gameObject.SetActive(true);
            _startPos = eventData.position;
            joystickParantImage.localPosition = eventData.position;
            joystickKnob.localPosition = eventData.position - _startPos;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (IsTouching)
            {
                Vector2 newPos = eventData.position - _startPos;
                float distance = Vector2.Distance(_startPos, eventData.position);

                if (distance > joystickRange)
                {
                    float normal = distance - joystickRange;
                    Vector2 prevVec = newPos;
                    newPos *= 1 - (normal / distance);
                }

                joystickKnob.localPosition = newPos;
                Vector3 dir = new Vector3(newPos.x, 0, newPos.y).normalized;
                if (_movePos != dir)
                {
                    OnMoveDirectionChangedEvent?.Invoke(_movePos,dir);
                    _movePos = dir;
                }
            }
        }

    }
}