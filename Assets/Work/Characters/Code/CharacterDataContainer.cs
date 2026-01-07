using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;
using Work.Inputs;

namespace Work.Characters
{
    public class CharacterDataContainer : MonoBehaviour //다른 곳에서 플레이어의 데이터가 필요할때 플레이어를 직접적으로 참조하는것이 아니라 이곳에서 데이터만 가지고 간다.
    {
        public Character CurrentCharacter { get; private set; }
        public Character characterPrefab;
        public CharacterClassDataSO CurrentCharacterData { get; private set; }
        public StatContainer CurrentCharacterStats => CurrentCharacter != null ? CurrentCharacter.StatContainer : null;
        public Vector3 MoveDirection { get; private set; }

        public InputContainer InputContainer { get; private set; }

        private void Awake()
        {
            InputContainer = new InputContainer();
            InputContainer.Init();
        }

        public void SetCharacterData(CharacterClassDataSO characterData) //탑에 들어가기 전에만 호출될거니까 뭐 할필요 없겠지..?
        {
            CurrentCharacterData = characterData;
        }
    }
}