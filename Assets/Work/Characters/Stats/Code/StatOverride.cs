using System;
using UnityEngine;

namespace Work.Characters.Stats.Code
{
    [Serializable]
    public class StatOverride
    {
        public string name;
        [SerializeField] private StatValueSO stat;
        [SerializeField] private bool isOverrid;
        [SerializeField] private float overridValue;

        public CharacterStat CreateStat()
        {
            StatContext context;
            if (isOverrid)
                context = this.stat.GenerateStatContext(overridValue);
            else
                context = this.stat.GenerateStatContext();
            CharacterStat stat = new CharacterStat(context);

            return stat;
        }

        
    }
}
