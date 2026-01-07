using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;
using Work.Entities.Code;

namespace Work.Combat
{
    [CreateAssetMenu(fileName = "CombatEntityData" , menuName = "SO/Combat/CombatEntityData")]
    public class CombatEntityDataSO : AbstractEntityDataSO
    {
        [field: SerializeField] public AttackDataSO[] attackDatas { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }
        [field: SerializeField] public ChracterAttackRangeTypeEnum AttackRangeType { get; private set; }
        [SerializeField] private List<StatOverride> stats;

        public Dictionary<string, Stat> GetDefaultStats()
        {
            Dictionary<string, Stat> realStats = new Dictionary<string, Stat>();
            foreach (StatOverride item in stats)
            {
                Stat stat = item.CreateStat();
                realStats.Add(stat.StatContext.StatName, stat);
            }

            return realStats;
        }
    }
}