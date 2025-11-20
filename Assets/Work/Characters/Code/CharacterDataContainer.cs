using System;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;
using Work.Joystick.Code;

namespace Work.Characters
{
    public class CharacterDataContainer : MonoBehaviour
    {
        public CharacterDataSO CurrentCharacterData {  get; private set; }
        public CharacterStatContainer CharacterStatContainer { get; private set; }
        public Vector3 MoveDirection { get; private set; }
        public Action OnMoveDirectionChanged;

        public JoystikcHandler JoystickHandler; //이거는 나중에 따로 빼야할듯


        private void Awake()
        {
            CharacterStatContainer = new CharacterStatContainer();
            JoystickHandler.OnMoveDirectionChangedEvent += HandleMoveDirectionChangeEvent;
        }

        private void HandleMoveDirectionChangeEvent(Vector3 prev, Vector3 current)
        {
            MoveDirection = current;
            OnMoveDirectionChanged?.Invoke();
        }

        public void SetCharacterData(CharacterDataSO characterData) //탑에 들어가기 전에만 호출될거니까 뭐 할필요 없겠지..?
        {
            CurrentCharacterData = characterData;
            CharacterStatContainer.InitailizeStatContainer(CurrentCharacterData);
        }
    }
}