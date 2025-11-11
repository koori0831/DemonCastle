using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Stats.Code;

namespace Work.Characters.Code
{
    [CreateAssetMenu(fileName = " Character data", menuName = "SO/Characters/CharacterData", order = -100)]
    public class CharacterDataSO : ScriptableObject
    {
        //스킬 , 고유경험등 이것저것 추가예정
       
        [field: SerializeField] public string CharacterName;
        [field: SerializeField] public string CharacterDescription;

        [SerializeField] private List<StatOverride> stats;

        public Dictionary<string, CharacterStat> GetStats()
        {
            Dictionary<string, CharacterStat> realStats = new Dictionary<string, CharacterStat>();
            foreach (StatOverride item in stats)
            {
                CharacterStat stat = item.CreateStat();
                realStats.Add(stat.StatContext.StatName, stat);
            }

            return realStats;
        }
    }
}
