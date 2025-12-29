using System;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.Players
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject
    {
        [SerializeField] private LayerMask whatIsGround;
        //public event Action<Vector2> OnMovementChange;
        public event Action OnAttackPressed;
        public event Action OnRollingPressed;
        public event Action<bool> OnSkillPressed;
        
        public Vector2 MovementKey { get; private set; }
        
        private Vector2 _screenPosition;
        private Vector3 _worldPosition;


        public void OnMove(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackPressed?.Invoke();
        }

        public void OnRolling(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnRollingPressed?.Invoke();
        }

        public void OnPointer(InputAction.CallbackContext context)
        {
            _screenPosition = context.ReadValue<Vector2>();
        }

        public void OnSkill(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSkillPressed?.Invoke(true);
            else if(context.canceled)
                OnSkillPressed?.Invoke(false);
        }

        public Vector3 GetWorldPosition()
        {
            Camera mainCam = Camera.main; //유니티 2022부터는 내부 캐싱하기 때문에 이렇게 써도 돼.
            Debug.Assert(mainCam != null, "No main camera in this scene.");
            Ray cameraRay = mainCam.ScreenPointToRay(_screenPosition);
            if (Physics.Raycast(cameraRay, out RaycastHit hit, mainCam.farClipPlane, whatIsGround))
            {
                _worldPosition = hit.point;
            }
            return _worldPosition;
        }


    }
}