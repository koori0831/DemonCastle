using System;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using Blade.Players;
using Blade.SoundSystem;
using UnityEngine;

namespace Blade.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public class GoldBag : MonoBehaviour, ICollectable
    {
        public bool CanCollect { get; private set; }

        [SerializeField] private int goldAmount;
        [SerializeField] private ItemEffect itemEffect;
        [SerializeField] private float dropDuration = 2f;
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private SoundSO collectSound;
        
        private Rigidbody _rigidbody;
        private float _dropTime;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            itemEffect.SetItemEffect(false);
            _dropTime = 0;
        }

        private void FixedUpdate()
        {
            _dropTime += Time.fixedDeltaTime;
            if (itemEffect.gameObject.activeSelf == false && CanCollect
                                               && _rigidbody.linearVelocity.magnitude < 0.2f)
            {
                itemEffect.SetItemEffect(true);
            }

            if (_dropTime >= dropDuration && itemEffect.gameObject.activeSelf == false)
            {
                _rigidbody.linearVelocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                itemEffect.SetItemEffect(true);
            }
        }

        public void SetGoldAmount(int amount)
        {
            goldAmount = amount;
            CanCollect = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            CanCollect = true;
        }

        public void Collect(Entity entity)
        {
            if (entity.CompareTag("Player") == false) return;
            soundChannel.RaiseEvent(SoundEvents.PlaySFXEvent.Initialize(transform.position, collectSound));
            
            CanCollect = false;
            PlayerData playerData = entity.GetCompo<PlayerData>();
            playerData.AddGold(goldAmount);
            Destroy(gameObject);
        }

        public void AddForceToGoldBag(Vector3 force)
        {
            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}