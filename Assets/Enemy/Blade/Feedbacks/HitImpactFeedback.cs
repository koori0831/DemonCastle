using Blade.Combat;
using Blade.Effects;
using Blade.Entities;
using DG.Tweening;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.Feedbacks
{
    public class HitImpactFeedback : Feedback
    {
        [SerializeField] private PoolItemSO impactItem;
        [SerializeField] private PoolItemSO slashItem;
        [SerializeField] private float playDuration;
        [SerializeField] private EntityActionData actionData;
        [SerializeField] private DamageType allowedDamageType;

        [Inject] private PoolManagerMono _poolManager;
        
        public override void CreateFeedback()
        {
            if ((actionData.LastDamageData.damageType & allowedDamageType) == 0)
                return;
            
            var effect = _poolManager.Pop<PoolingEffect>(impactItem);

            Quaternion rotation = Quaternion.LookRotation(actionData.HitNormal * -1); //노말의 반대방향으로 이펙트 재생
            effect.PlayVFX(actionData.HitPoint, rotation);

            if (slashItem != null)
            {
                var slashEffect = _poolManager.Pop<PoolingEffect>(slashItem);
                slashEffect.PlayVFX(actionData.HitPoint, Quaternion.identity);
                
                DOVirtual.DelayedCall(playDuration, () =>
                {
                    _poolManager.Push(slashEffect);
                });
            }
            
            DOVirtual.DelayedCall(playDuration, () =>
            {
                _poolManager.Push(effect);
            });
        }

        public override void StopFeedback()
        {
            
        }
    }
}