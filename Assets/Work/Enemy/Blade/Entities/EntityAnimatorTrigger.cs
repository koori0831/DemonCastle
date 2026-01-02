using System;
using UnityEngine;

namespace Blade.Entities
{
    public class EntityAnimatorTrigger : MonoBehaviour, IEntityComponent
    {
        public Action OnAnimationEndTrigger;
        public Action OnAttackVFXTrigger;
        // public Action<bool> OnRollingStatusChangeTrigger;
        public Action<bool> OnManualRotationTrigger;
        public Action OnDamageCastTrigger;
        public Action<bool> OnDamageToggleTrigger;
        public Action OnCastSkillTrigger; //이거 추가
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void StartManualRotation() => OnManualRotationTrigger?.Invoke(true);
        private void StopManualRotation() => OnManualRotationTrigger?.Invoke(false);
        private void AnimationEnd() => OnAnimationEndTrigger?.Invoke();
        // private void RollingStart() => OnRollingStatusChangeTrigger?.Invoke(true);
        // private void RollingEnd() => OnRollingStatusChangeTrigger?.Invoke(false);
        private void PlayAttackVFX() => OnAttackVFXTrigger?.Invoke();
        private void DamageCast() => OnDamageCastTrigger?.Invoke();
        private void StartDamageCast() => OnDamageToggleTrigger?.Invoke(true);
        private void StopDamageCast() => OnDamageToggleTrigger?.Invoke(false);
        private void CastSkill() => OnCastSkillTrigger?.Invoke();

    }
}