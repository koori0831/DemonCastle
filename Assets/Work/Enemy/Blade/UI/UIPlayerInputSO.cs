using System;
using Blade.Players;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Blade.UI
{
    [CreateAssetMenu(fileName = "UI player input", menuName = "SO/UIPlayerInput", order = 0)]
    public class UIPlayerInputSO : ScriptableObject
    {
        [SerializeField] private PlayerInputSO playerInput;

        public event Action OnMenuButtonPress;

        private void OnDisable()
        {
        }

        public void OnMenuButton(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnMenuButtonPress?.Invoke();
        }
    }
}