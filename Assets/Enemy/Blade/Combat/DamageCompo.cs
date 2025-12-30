using Blade.Entities;
using Blade.StatSystem;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Blade.Combat
{
    public class DamageCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [SerializeField] private StatSO criticalStat, criticalDamageStat;

        private Entity _entity;
        private EntityStatCompo _statCompo;

        private float _critical, _criticalDamage;
            
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }

        public void AfterInitialize()
        {
            if (criticalStat == null)
                _critical = 0;
            else
                _critical = _statCompo.SubscribeStat(criticalStat, HandleCriticalChange, 0);
            
            if (criticalDamageStat == null)
                _criticalDamage = 1f; //기본값은 1배
            else
                _criticalDamage = _statCompo.SubscribeStat(criticalDamageStat, 
                    HandleCriticalDamageChange, 1f);
            
        }

        private void OnDestroy()
        {
            //귀찮으니까 나중에 개선하고 다시 작성할께.
            if(criticalStat != null)
                _statCompo.UnSubscribeStat(criticalStat, HandleCriticalChange);
            if(criticalDamageStat != null)
                _statCompo.UnSubscribeStat(criticalDamageStat, HandleCriticalDamageChange);
        }

        private void HandleCriticalChange(StatSO stat, float currentvalue, float previousvalue)
            => _critical = currentvalue;
        
        private void HandleCriticalDamageChange(StatSO stat, float currentvalue, float previousvalue)
            => _criticalDamage = currentvalue;

        public DamageData CalculateDamage(StatSO majorStat, AttackDataSO attackData, float multiplier = 1f)
        {
            DamageData damageData = new DamageData();
            damageData.damage = _statCompo.GetStat(majorStat).Value * attackData.damageMultiplier
                                + attackData.damageIncrease * multiplier;
            if (Random.value < _critical)
            {
                damageData.damage *= _criticalDamage; //크리티컬 증뎀만큼 증가시켜주고
                damageData.isCritical = true;
            }
            else
            {
                damageData.isCritical = false;
            }

            damageData.damageType = attackData.damageType;

            return damageData;
        }
    }
}