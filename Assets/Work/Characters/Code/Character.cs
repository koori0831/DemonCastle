using System.Collections;
using UnityEngine;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Characters.Code
{
    public class Character : Entity
    {
        public CharacterDataContainer CharacterDataContainer { get; private set; }
        public CharacterDataSO CharacterData => CharacterDataContainer.CurrentCharacterData;

        public void Initialized(CharacterDataContainer currentData)
        {
            CharacterDataContainer = currentData;
            Init(CharacterData as AbstractEntityDataSO);
        }
    }
}