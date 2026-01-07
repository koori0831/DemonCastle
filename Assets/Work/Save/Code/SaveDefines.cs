using System;
using System.Collections.Generic;
using Work.Characters.Stats.Code;

namespace Work.Save.Code
{
	[Serializable]
	public class CharacterData
    {
        public string characterId;  // Guid.ToString()
        public string name;

        public int classId;      // ClassDefinitionSO.id
        public StatBlock modifiers;
    }

    [Serializable]
    public class CharacterSaveData
    {
        public List<CharacterData> characters = new();
        public string selectedCharacterId; // 현재 선택된 캐릭터

        internal CharacterData GetCharacterDataByID(int characterID)
        {
            CharacterData data = characters.Find(c => c.characterId == characterID.ToString());
            return data;
        }

        public static CharacterSaveData operator +(CharacterSaveData saveData, CharacterData newData)
        {
            // Check if character with the same ID already exists
            int index = saveData.characters.FindIndex(c => c.characterId == newData.characterId);
            if (index >= 0)
            {
                // Update existing character data
                saveData.characters[index] = newData;
            }
            else
            {
                // Add new character data
                saveData.characters.Add(newData);
            }
            return saveData;
        }
    }
}