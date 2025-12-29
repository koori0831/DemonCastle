using System.Collections.Generic;
using Blade.SkillSystem.Upgrade;
using UnityEngine;

namespace Blade.SkillSystem
{
    [CreateAssetMenu(fileName = "Skill data", menuName = "SO/Combat/Skill data", order = 0)]
    public class SkillDataSO : ScriptableObject
    {
        public string skillName;
        public Sprite skillIcon;
        public List<SkillUpgradeSO> upgradeList;
    }
}