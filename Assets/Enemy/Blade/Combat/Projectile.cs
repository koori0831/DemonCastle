using System;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using Blade.SoundSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.Combat
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private AttackDataSO attackData;
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private PoolItemSO impactEffect;
        [SerializeField] private GameEventChannelSO effectChannel;
        [SerializeField] private GameEventChannelSO cameraChannel;
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private SoundSO explosionSound;
        
        private Entity _owner;
        private Pool _myPool;
        private DamageData _damageData;
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        public void SetupProjectile(Entity entity, DamageData damageData, Vector3 position, Quaternion rotation,
            Vector3 velocity)
        {
            _owner = entity;
            _damageData = damageData;
            transform.SetPositionAndRotation(position, rotation);
            rigidbody.linearVelocity = velocity;
            
            damageCaster.InitCaster(_owner);
        }

        private void OnTriggerEnter(Collider other)
        {
            damageCaster.CastDamage(_damageData, transform.position, transform.forward, attackData);

            var effectEvt = EffectEvents.PlayPoolEffect.Initializer(
                transform.position, Quaternion.identity, impactEffect, 2f);
            effectChannel.RaiseEvent(effectEvt);

            if (explosionSound != null)
            {
                var soundEvt = SoundEvents.PlaySFXEvent.Initialize(transform.position, explosionSound);
                soundChannel.RaiseEvent(soundEvt);
            }
            
            if (attackData.impulseForce > 0)
            {
                var impulseEvt = CameraEvents.ImpulseEvent.Initializer(attackData.impulseForce);
                cameraChannel.RaiseEvent(impulseEvt);
            }
            
            _myPool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            
        }
    }
}

/*
숙제 : 다다음주 월요일
각자 자기만의 스킬 하나 만들어오기
1등은 가산점 5점 - 1명
2등은 3점 - 2명
3등은 1점, 무제한
*/