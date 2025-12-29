using System.Collections;
using UnityEngine;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Characters.Code
{
    public class Character : Entity
    {
        public CharacterDataSO CharacterData => EntityDataSO as CharacterDataSO;
    }
}