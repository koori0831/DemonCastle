using Blade.Combat;
using Blade.Entities;
using Blade.Events;
using Blade.Players;
using Blade.SoundSystem;
using Blade.StatSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.SkillSystem
{
    public class BlueBulletSkill : Skill
    {
        [SerializeField] private ParticleSystem[] muzzleEffects;
        [SerializeField] private PoolItemSO blueBullet;
        [SerializeField] private AttackDataSO bulletData;
        [SerializeField] private StatSO damageStat;
        [SerializeField] private float damageMultiplier = 1f;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private PlayerInputSO playerInput;
        
        [SerializeField] private SoundSO launchSound;
        
        [Inject] private PoolManagerMono _poolManager;
        private DamageCompo _damageCompo;
        private EntityAnimatorTrigger _trigger;
        private int _muzzleIndex;

        public override void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            base.InitializeSkill(owner, skillComponent);
            _trigger = owner.GetCompo<EntityAnimatorTrigger>();
            _damageCompo = owner.GetCompo<DamageCompo>();
        }

        public override void UseSkill()
        {
            // base.UseSkill();
            _muzzleIndex = 0;
            _trigger.OnCastSkillTrigger += HandleSkillTrigger;
            Vector3 worldPosition = playerInput.GetWorldPosition();
            _owner.RotateToTarget(worldPosition);
        }

        private void HandleSkillTrigger()
        {
            if (_muzzleIndex >= muzzleEffects.Length) return;
            ParticleSystem targetParticle = muzzleEffects[_muzzleIndex];
            targetParticle.Play();
            Transform targetTrm = targetParticle.transform;
            
            //여기서 실질적인 발사체를 발사하게 된다.
            Projectile bullet = _poolManager.Pop<Projectile>(blueBullet);
            DamageData damageData = _damageCompo.CalculateDamage(damageStat, bulletData, damageMultiplier);
            
            Quaternion forwardRot = Quaternion.LookRotation(targetTrm.forward);
            bullet.SetupProjectile(_owner, damageData, targetTrm.position, forwardRot, targetTrm.forward * bulletSpeed);

            PlaySFXEvent evt = SoundEvents.PlaySFXEvent.Initialize(targetTrm.position, launchSound);
            _skillComponent.SoundChannel.RaiseEvent(evt);
            
            _muzzleIndex++;
            if (_muzzleIndex >= muzzleEffects.Length)
            {
                base.UseSkill();
                _trigger.OnCastSkillTrigger -= HandleSkillTrigger;
            }
        }
    }
}