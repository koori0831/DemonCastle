using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;

namespace Work.Entities.Code
{
    [Serializable]
    public class AnimationData
    {
        public RuntimeAnimatorController AnimatorController;
        public GameObject visualPrefab;
    }

    public abstract class AbstractEntityDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public float AttackRange { get; private set; }  
        [field: SerializeField] public ChracterAttackRangeTypeEnum AttackRangeType { get; private set; }
        [field: SerializeField] public AnimationData AnimationData { get; private set; }

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
