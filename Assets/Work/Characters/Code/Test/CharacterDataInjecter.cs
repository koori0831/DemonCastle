using Blade.Managers;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.Characters.Code.Test
{
    [DefaultExecutionOrder(-2)]
    public class CharacterDataInjecter : MonoBehaviour
    {
        [SerializeField] private CharacterDataSO characterDataSO;
        [SerializeField] private CharacterDataContainer characterDataContainer;
        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private CinemachineCamera playerFollowCam;
        [SerializeField] private PlayerManager playerManager;

        

        private void Awake()
        {
            TestFunc();
            CharacterCreateTestFunc();
            
        }

        public void Start()
        {
            
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

            playerManager._player = character;
        }
    }
}