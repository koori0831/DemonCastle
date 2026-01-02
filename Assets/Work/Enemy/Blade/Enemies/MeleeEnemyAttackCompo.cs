using System;
using Blade.Combat;
using Blade.Entities;
using Blade.StatSystem;
using UnityEngine;

namespace Blade.Enemies
{
    public class MeleeEnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        private Entity _entity;
        private DamageCompo _damageCompo;
        private EntityStatCompo _statCompo;
        private EntityAnimatorTrigger _trigger;

        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private StatSO meleeDamageStat;
        [SerializeField] private OverlapDamageCaster[] casters;

        private bool _isActive;
        private DamageData _currentDamageData;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
            _trigger = entity.GetCompo<EntityAnimatorTrigger>();

            casters = entity.GetComponentsInChildren<OverlapDamageCaster>(true);
            foreach (var caster in casters)
            {
                caster.InitCaster(entity);
            }
        }

        public void AfterInitialize()
        {
            meleeDamageStat = _statCompo.GetStat(meleeDamageStat);
            _trigger.OnDamageToggleTrigger += SetDamageCaster;
        }

        private void OnDestroy()
        {
            _trigger.OnDamageToggleTrigger -= SetDamageCaster;
        }

        public void SetDamageCaster(bool isActive)
        {
            _isActive = isActive;
            if (isActive)
            {
                foreach (var caster in casters)
                {
                    caster.StartCasting(); //데미지 캐스팅 시작
                }
                
                _currentDamageData = _damageCompo.CalculateDamage(meleeDamageStat, attackData);
            }
        }

        private void FixedUpdate()
        {
            if (_isActive)
            {
                foreach (var caster in casters)
                {
                    caster.CastDamage(_currentDamageData, transform.position, 
                        transform.forward, attackData);
                }
            }
        }

        
    }
}