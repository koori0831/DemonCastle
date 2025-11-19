using UnityEngine;

namespace Work.Characters.Code.Test
{
    public class CharacterDataInjecter : MonoBehaviour
    {
        [SerializeField] private CharacterDataSO characterDataSO;
        [SerializeField] private CharacterDataContainer characterDataContainer;
        [SerializeField] private GameObject characterPrefab;

        [ContextMenu("Test Character Data Set")]
        public void TestFunc()
        {
            characterDataContainer.SetCharacterData(characterDataSO);
        }

        [ContextMenu("Test Character Create")]
        public void CharacterCreateTestFunc()
        {
            Character character = Instantiate(characterPrefab).GetComponent<Character>();
            character.Initialized(characterDataContainer);
        }
    }
}