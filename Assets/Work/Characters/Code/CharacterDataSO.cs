using System.Collections.Generic;
using UnityEngine;
using Work.Characters.FSM.Code;
using Work.Combat;
using Work.Entities.Code;

namespace Work.Characters.Code
{
    

    [CreateAssetMenu(fileName = " Character data", menuName = "SO/Characters/CharacterData", order = -100)]
    public class CharacterDataSO : AbstractEntityDataSO
    {
        //스킬 , 고유경험등 이것저것 추가예정
        [field: SerializeField] public int ClassID {  get; private set; }
        [field: SerializeField] public CharacterEnum CharacterType { get; private set; }
        [field: SerializeField] public List<StateSO> stateSOs { get; private set; }
        [field: SerializeField] public AttackDataSO[] attackDatas { get; private set; }
        [field: SerializeField] public SkillData[] skillDatas { get; private set; }
    }
}
