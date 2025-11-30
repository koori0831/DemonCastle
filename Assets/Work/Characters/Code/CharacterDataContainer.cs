using System;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;
using Work.Joystick.Code;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

namespace Work.Characters
{
    public class CharacterDataContainer : MonoBehaviour //다른 곳에서 플레이어의 데이터가 필요할때 플레이어를 직접적으로 참조하는것이 아니라 이곳에서 데이터만 가지고 간다.
    {
        public Character CurrentCharacter { get; private set; }
        public Character characterPrefab;
        public CharacterDataSO CurrentCharacterData {  get; private set; }
        public Vector3 MoveDirection { get; private set; }

        public JoystikcHandler JoystickHandler; //이거는 나중에 따로 빼야할듯

        private void Awake()
        {
            JoystickHandler.OnMoveDirectionChangedEvent += HandleMoveDirectionChangeEvent;
        }

        private void HandleMoveDirectionChangeEvent(Vector3 prev, Vector3 current)
        {
            MoveDirection = current;
            Bus<PlayerMoveEvent>.Raise(new PlayerMoveEvent(MoveDirection));
        }

        public void SetCharacterData(CharacterDataSO characterData) //탑에 들어가기 전에만 호출될거니까 뭐 할필요 없겠지..?
        {
            CurrentCharacterData = characterData;
        }
    }
}