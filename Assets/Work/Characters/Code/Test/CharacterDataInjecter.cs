using Unity.Cinemachine;
using UnityEngine;

namespace Work.Characters.Code.Test
{
    public class CharacterDataInjecter : MonoBehaviour
    {
        [SerializeField] private CharacterDataSO characterDataSO;
        [SerializeField] private CharacterDataContainer characterDataContainer;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private CinemachineCamera playerFollowCam;

        public void Start()
        {
            TestFunc();
            CharacterCreateTestFunc();
        }

        [ContextMenu("Test Character Data Set")]
        public void TestFunc()
        {
            characterDataContainer.SetCharacterData(characterDataSO);
        }

        [ContextMenu("Test Character Create")]
        public void CharacterCreateTestFunc()
        {
            Character character = Instantiate(characterPrefab).GetComponent<Character>();
            character.Init(characterDataSO);

            playerFollowCam.Target.TrackingTarget = character.transform;
        }
    }
}