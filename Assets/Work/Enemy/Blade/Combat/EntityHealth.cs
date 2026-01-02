using System;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using Blade.StatSystem;
using Blade.UI;
using UnityEngine;

namespace Blade.Combat
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IDamageable, IAfterInitialize
    {
        private Entity _entity;
        private EntityActionData _actionData;
        private EntityStatCompo _statCompo;

        [SerializeField] private StatSO hpStat;
        [SerializeField] private float maxHealth;
        [SerializeField] private float currentHealth;
        [SerializeField] private GameEventChannelSO effectChannel;
        [SerializeField] private TextInfoSO normalText, criticalText;

        public delegate void HealthChange(float current, float max);
        public event HealthChange OnHealthChange;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _actionData = entity.GetCompo<EntityActionData>();
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }
        
        public void AfterInitialize()
        {
            currentHealth = maxHealth = _statCompo.SubscribeStat(hpStat, HandleHPValueChange, 1);
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(hpStat, HandleHPValueChange);
        }

        private void HandleHPValueChange(StatSO stat, float currentvalue, float previousvalue)
        {
            float changed = currentvalue - previousvalue;
            maxHealth = currentvalue;
            currentHealth = Mathf.Clamp(currentHealth + changed, 1, maxHealth);
            OnHealthChange?.Invoke(currentHealth, maxHealth);
        }

        public void ApplyDamage(DamageData damageData, Vector3 hitPoint, Vector3 normal, AttackDataSO attackData, Entity dealer)
        {
            _actionData.HitPoint = hitPoint;
            _actionData.HitNormal = normal;
            _actionData.HitByPowerAttack = attackData.isPowerAttack;
            _actionData.LastDamageData = damageData;
            
            currentHealth = Mathf.Clamp(currentHealth - damageData.damage, 0, maxHealth);
            OnHealthChange?.Invoke(currentHealth, maxHealth);
            
            int typeHash = damageData.isCritical ? criticalText.nameHash : normalText.nameHash;
            Vector3 position = hitPoint + new Vector3(0, 1.5f);
            PopupTextEvent evt = EffectEvents.PopupTextEvent.Initialize(
                damageData.damage.ToString(), typeHash, position, 0.5f);
            effectChannel.RaiseEvent(evt);
            
            if (currentHealth <= 0)
            {
                _entity.OnDieEvent?.Invoke();
            }
            
            _entity.OnHitEvent?.Invoke();
            
        }

        
    }
}