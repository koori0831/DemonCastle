using System;
using System.Collections.Generic;
using Work.Characters.Code;

namespace Work.Characters.Stats.Code
{
    public class CharacterStatContainer
    {
        #region Member

        private Dictionary<string, CharacterStat> stats;

        #endregion

        #region Initailizer

        public void InitailizeStatContainer(CharacterDataSO characterData)
        {
            if (characterData == null) return;
            SetStatDict(characterData);
        }

        #endregion

        #region Method

        private void SetStatDict(CharacterDataSO characterData)
        {
            Dictionary<string, CharacterStat> statDict = characterData.GetDefaultStats();
            if (statDict == null) return;
            stats = statDict;
        }

        public CharacterStat GetStat(string statName)
        {
            if (stats.TryGetValue(statName, out CharacterStat stat))
            {
                return stat;
            }
            return default;
        }

        public float GetStatValue(string statName)
        {
            CharacterStat stat = GetStat(statName);
            if (stat == null) return 0f;
            return stat.Value;
        }

        public void AddListenerValueChangedEvent(Action<float, float> action, string statName)
        {
            CharacterStat stat = GetStat(statName);
            if (stat == null) return;
            stat.AddListenerValueChanged(action);
        }

        public void RemoveListenerValueChangedEvent(Action<float, float> action, string statName)
        {
            CharacterStat stat = GetStat(statName);
            if (stat == null) return;
            stat.RemoveListenerValueChanged(action);
        }

        public void RemoveAllValueChangedEvent(string statName)
        {
            CharacterStat stat = GetStat(statName);
            if (stat == null) return;
            stat.RemoveAllListenerValueChanged();
        }

        public void AddModifier(string statName, IStatUpgrader statUpgrader, float value)
        {
            CharacterStat stat = GetStat(statName);
            if (stat == null || statUpgrader == null) return;
            stat.AddModifier(statUpgrader, value);
        }

        public void RemoveModifier(string statName, IStatUpgrader statUpgrader)
        {
            CharacterStat stat = GetStat(statName);
            if (stat == null || statUpgrader == null) return;
            stat.RemoveModifier(statUpgrader);
        }

        #endregion
    }
}