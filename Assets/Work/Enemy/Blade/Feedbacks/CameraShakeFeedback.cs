using System;
using Blade.Entities;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Feedbacks
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraShakeFeedback : Feedback
    {
        [SerializeField] private EntityActionData actionData;
        [SerializeField] private float impulseForce = 0.8f;
        private CinemachineImpulseSource _impulseSource;
        
        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        public override void CreateFeedback()
        {
            if(actionData.HitByPowerAttack)
                _impulseSource.GenerateImpulse(impulseForce);
        }

        public override void StopFeedback()
        {
        }
    }
}