using UnityEngine;
using UnityEngine.InputSystem;
using Work.Characters.Events;
using Work.Utils.EventBus;

namespace Work.Inputs
{
    public class InputContainer : Console.IPlayerActions
    {
        private Console _console;

        public void Init()
        {
            if (_console == null)
            {
                _console = new Console();
                _console.Player.SetCallbacks(this);
            }
            _console.Player.Enable();
        }

        ~InputContainer()
        {
            _console.Player.Disable();
            _console = null;
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

            Bus<CharacterMoveEvent>.Raise(new CharacterMoveEvent(new Vector3( vector.x,0, vector.y)));
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
    }
}