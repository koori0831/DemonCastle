using UnityEngine;
using UnityEngine.EventSystems;

namespace Work.Joystick.Code
{
    public class JoystikcHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public bool IsTouching { get; private set; } = false;
        [SerializeField] private GameObject joystickKnob;
        [SerializeField] private GameObject joystickParantImage;
        [SerializeField] private float joystickRange = 50f;
        private Vector2 _startPos;
        [SerializeField ]private Vector3 _movePos;

        public void OnPointerUp(PointerEventData eventData)
        {
            IsTouching = false;
            joystickParantImage.SetActive(false);
            joystickKnob.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsTouching = true;
            joystickParantImage.SetActive(true);
            joystickKnob.SetActive(true);
            joystickParantImage.transform.position = eventData.position;
            joystickKnob.transform.position = eventData.position;
            _startPos = eventData.position;
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if(IsTouching)
            {
                Vector2 newPos = eventData.position - _startPos;
                float distance = Vector2.Distance(_startPos, eventData.position);
                if (distance > joystickRange) 
                {
                    float normal = distance - joystickRange;
                    Vector2 prevVec = newPos;
                    newPos *= 1 - (normal / distance);
                }

                joystickKnob.transform.position = _startPos + newPos;
                Vector3 dir = new Vector3(newPos.x,0,newPos.y);
                _movePos = dir;
            }
        }

    }
}