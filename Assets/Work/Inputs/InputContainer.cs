using UnityEngine;
using UnityEngine.InputSystem;
using Work.Characters.Events;
using Work.Utils.EventBus;
using Work.Utils.Helpers;

namespace Work.Inputs
{
    public class InputContainer : Console.IPlayerActions
    {
        private Console _console;

        private Vector3 _mousePosition;

        public void Init()
        {
            if (_console == null)
            {
                _console = new Console();
                _console.Player.SetCallbacks(this);
            }
            _console.Player.Enable();
            Bus<CharacterInputEnableEvent>.Events += SetEnable;
        }

        ~InputContainer()
        {
            _console.Player.Disable();
            _console = null;
            Bus<CharacterInputEnableEvent>.Events -= SetEnable;
        }

        public void SetEnable(CharacterInputEnableEvent evt)
        {
            if (evt.Enable)
                _console.Player.Enable();
            else
                _console.Player.Disable();
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<CharacterDashEvent>.Raise(new CharacterDashEvent());
        }

        public void OnGung(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<CharacterUltimateSkillEvent>.Raise(new CharacterUltimateSkillEvent());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<CharacterInteractionEvent>.Raise(new CharacterInteractionEvent());
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 vector = context.ReadValue<Vector2>();

            Bus<CharacterMoveEvent>.Raise(new CharacterMoveEvent(new Vector3(vector.x, 0, vector.y)));
        }

        public void OnSkill_1(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<CharacterSkillEvent>.Raise(new CharacterSkillEvent(1));
        }

        public void OnSkill_2(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<CharacterSkillEvent>.Raise(new CharacterSkillEvent(2));
        }

        public void OnLeftClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<MouseClickEvent>.Raise(new MouseClickEvent(GetClickData(MouseClickType.Left)));
        }

        public void OnRightClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                Bus<MouseClickEvent>.Raise(new MouseClickEvent(GetClickData(MouseClickType.Right)));
        }

        public void OnMousePos(InputAction.CallbackContext context)
        {
            _mousePosition = context.ReadValue<Vector2>();
        }

        public Vector3 GetHitPointToWorld()
        {
            Camera mainCam = Camera.main;
            Ray cameraRay = mainCam.ScreenPointToRay(_mousePosition);
            if (Physics.Raycast(cameraRay, out RaycastHit hit, mainCam.farClipPlane))
            {
                return hit.point;
            }
            return Vector3.zero;
        }

        public ClickData GetClickData(MouseClickType clickType)
        {
            RaycastHit hit = GetHit();

            ClickData clickData = new ClickData(hit, clickType);
            return clickData;
        }

        private RaycastHit GetHit()
        {
            Camera mainCam = Camera.main;
            Ray cameraRay = mainCam.ScreenPointToRay(_mousePosition);
            Physics.Raycast(cameraRay, out RaycastHit hit, mainCam.farClipPlane);
            return hit;
        }


    }
}