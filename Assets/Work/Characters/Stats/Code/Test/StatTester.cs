using UnityEngine;
using Work.Characters.Code;

namespace Work.Characters.Stats.Code.Test
{
    public class StatTester : MonoBehaviour, IStatUpgrader
    {
        [SerializeField] private CharacterDataSO characterData;
        [SerializeField] private string statName = "HP";
        [SerializeField] private float addValue = 10;
        [SerializeField] private CharacterDataContainer chareacterDataContainer;

        private void Start()
        {
            chareacterDataContainer.SetCharacterData(characterData);
            chareacterDataContainer.CharacterStatContainer.AddListenerValueChangedEvent(Handler, statName);
            Debug.Log($"½ºÅÈ : {statName}");
        }

        [ContextMenu("AddTest")]
        public void AddTest()
        {
            chareacterDataContainer.CharacterStatContainer.AddModifier(statName, this, addValue);
        }

        [ContextMenu("RemoveTest")]
        public void RemoveTest()
        {
            chareacterDataContainer.CharacterStatContainer.RemoveModifier(statName, this);
        }

        [ContextMenu("GetTest")]
        public void GetTest()
        {
            Debug.Log(chareacterDataContainer.CharacterStatContainer.GetStatValue(statName));
        }

        private void Handler(float prev, float change)
        {
            Debug.Log($"ÀÌÀü°ª : {prev}");
            Debug.Log($"¹Ù²ï°ª : {change}");
        }
    }
}