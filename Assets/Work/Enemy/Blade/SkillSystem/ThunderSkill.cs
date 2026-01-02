using System.Net.NetworkInformation;
using Blade.Combat;
using Blade.Effects;
using Blade.Entities;
using Blade.Events;
using Blade.SoundSystem;
using Blade.StatSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.SkillSystem
{
    public class ThunderSkill : Skill, IChargeable
    {
        [SerializeField] private PoolItemSO thunderEffect;
        [SerializeField] private StatSO damageStat;
        [SerializeField] private AttackDataSO thunderData;
        [SerializeField] private float skillDamageMultiplier = 1.2f;
        [SerializeField] private RoundDecal decal;
        [SerializeField] private float chargeSpeed = 2f;
        [SerializeField] private float maxRadius = 3f;
        [SerializeField] private SoundSO thunderSound;
        
        public bool IsCharging { get; set; }

        private float _currentRadius;
        private DamageCompo _damageCompo;
        private EntityStatCompo _statCompo;
        [Inject] private PoolManagerMono _poolManager;

        private StatSO _realStat;
        public override void InitializeSkill(Entity owner, SkillComponent skillComponent)
        {
            base.InitializeSkill(owner, skillComponent);
            decal.SetProjectActive(false); //처음시작하면 꺼준다.
            _damageCompo = owner.GetCompo<DamageCompo>();
            _statCompo = owner.GetCompo<EntityStatCompo>();
            _realStat = _statCompo.GetStat(damageStat);
        }

        public override void UseSkill()
        {
            base.UseSkill();

            int enemyCount = _skillComponent.GetEnemiesInRange(decal.transform.position, _currentRadius);

            for (int i = 0; i < enemyCount; i++)
            {
                Collider target = _skillComponent.Colliders[i];
                PoolingEffect effect = _poolManager.Pop<PoolingEffect>(thunderEffect);
                effect.PlayVFX(target.transform.position, Quaternion.identity);
                DelayedPooling(effect, 2f);

                var soundEvt = SoundEvents.PlaySFXEvent.Initialize(target.transform.position, thunderSound);
                _skillComponent.SoundChannel.RaiseEvent(soundEvt);
                
                if (target.TryGetComponent(out IDamageable damageable))
                {
                    
                    DamageData damageData =
                        _damageCompo.CalculateDamage(_realStat, thunderData, skillDamageMultiplier);
                    damageable.ApplyDamage(damageData, target.transform.position, Vector3.up,  thunderData, _owner);
                }
            }

            if (enemyCount > 0)
            {
                var impulseEvt = CameraEvents.ImpulseEvent.Initializer(thunderData.impulseForce);
                _skillComponent.CameraChannel.RaiseEvent(impulseEvt);
            }
            
        }

        private async void DelayedPooling(PoolingEffect effect, float duration)
        {
            await Awaitable.WaitForSecondsAsync(duration);
            _poolManager.Push(effect);
        }

        public void StartCharge()
        {
            _currentRadius = 0.1f;
            SetCharging(true);
        }

        public void ReleaseCharge()
        {
            SetCharging(false);
            UseSkill(); //떼는 순간 쿨타임이 가동되어야 하니까.
        }

        public void CancelCharge()
        {
            SetCharging(false);
        }
        
        private void SetCharging(bool isCharging)
        {
            decal.SetProjectActive(isCharging);
            IsCharging = isCharging;
        }

        protected override void Update()
        {
            base.Update();
            if (IsCharging)
            {
                _currentRadius += Time.deltaTime * chargeSpeed;
                _currentRadius = Mathf.Clamp(_currentRadius, 0, maxRadius);
                decal.SetDecalSize(_currentRadius);
            }
        }
    }
}