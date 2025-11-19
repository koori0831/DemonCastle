using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;

namespace Work.Entities.Code
{
    [Serializable]
    public class AnimationData
    {
        public AnimatorController AnimatorController;
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

        public Dictionary<string, CharacterStat> GetDefaultStats()
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
