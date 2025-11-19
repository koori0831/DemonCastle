using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;

namespace Work.Characters
{
    public class CharacterDataContainer : MonoBehaviour
    {
        public CharacterDataSO CurrentCharacterData {  get; private set; }
        public CharacterStatContainer CharacterStatContainer { get; private set; }

        private void Awake()
        {
            CharacterStatContainer = new CharacterStatContainer();
        }

        public void SetCharacterData(CharacterDataSO characterData) //탑에 들어가기 전에만 호출될거니까 뭐 할필요 없겠지..?
        {
            CurrentCharacterData = characterData;
            CharacterStatContainer.InitailizeStatContainer(CurrentCharacterData);
        }
    }
}