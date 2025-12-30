using System;
using System.Collections.Generic;
using UnityEngine;

namespace Blade.Combat
{
    [Serializable]
    public struct LevelData
    {
        public int level;
        public int requireExp;
    }
    
    [CreateAssetMenu(fileName = "Level table", menuName = "SO/Combat/Level table", order = 0)]
    public class LevelUpTableSO : ScriptableObject
    {
        public int maxLevel;
        public int statPerLevel = 4;
        public int skillPerLevel = 1;
        
        public List<LevelData> levelDataList;

        public int GetRequireExpForNextLevel(int nextLevel)
        {
            if (nextLevel > maxLevel) return -1;
            int index = levelDataList.FindIndex(data => data.level == nextLevel);
            if (index < 0) return -1;
            return levelDataList[index].requireExp;
        }
    }
}