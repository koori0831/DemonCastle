using System.Collections.Generic;
using System.Linq;
using Blade.Entities;
using Blade.SkillSystem.Upgrade;
using UnityEngine;

namespace Blade.SkillSystem
{
    public abstract class Skill : MonoBehaviour
    {
        public delegate void CooldownInfo(float current, float duration);

        [SerializeField] protected float cooldownDuration = 2f;
        
        protected float _cooldownTimer;
        protected Entity _owner;
        protected SkillComponent _skillComponent;

        public bool IsCooldown => _cooldownTimer > 0f;
        public event CooldownInfo OnCooldownInfo;

        public virtual void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            _owner = owner;
            _skillComponent = skillComponent;
        }

        protected virtual void Update()
        {
            if (_cooldownTimer <= 0) return;
            
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0)
            {
                _cooldownTimer = 0;
            }
            
            OnCooldownInfo?.Invoke(_cooldownTimer, cooldownDuration);
        }

        public virtual void UseSkill()
        {
            _cooldownTimer = cooldownDuration;
        }

        #region Upgrade skill section

        [field: SerializeField] public SkillDataSO SkillData { get; private set; }
        public List<SkillUpgradeSO> upgradedList = new();

        public int GetUpgradeCount(SkillUpgradeSO upgrade)
            => upgradedList.Count(data => data == upgrade); //Linq 넣어야 한다.

        public bool CanUpgradeSkill(SkillUpgradeSO upgrade)
        {
            foreach (var data in upgrade.needUpgradeList)
            {
                if (upgradedList.Contains(data) == false) return false;
            }

            foreach (var data in upgrade.dontNeedUpgradeList)
            {
                if (upgradedList.Contains(data)) return false;
            }
            
            int currentUpgradeCnt = GetUpgradeCount(upgrade);
            
            return currentUpgradeCnt < upgrade.maxUpgradeCount;
        }

        public void UpgradeSkill(SkillUpgradeSO upgrade)
        {
            upgradedList.Add(upgrade);
            upgrade.UpgradeSkill(this);
        }

        public void RollbackUpgrade(SkillUpgradeSO upgrade)
        {
            upgradedList.Remove(upgrade);
            upgrade.RollbackSkill(this);
        }

        #endregion
    }
}