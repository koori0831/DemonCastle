using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Work.Characters.Code
{
    [CreateAssetMenu(fileName = "CharacterClassDataList" , menuName = "SO/Characters/CharacterClassDataList")]
    public class CharacterClassDataListSO : ScriptableObject
    {
        [field: SerializeField] public List<CharacterClassDataSO> characterDataSOs {  get; private set; }

        public CharacterClassDataSO GetCharacterData(int id)
        {
            CharacterClassDataSO selectData = characterDataSOs.Find(x => x.ClassID == id);
            Debug.Assert(selectData != null, "Could not find character data for the corresponding ID");
            return selectData;
        }
    }
}