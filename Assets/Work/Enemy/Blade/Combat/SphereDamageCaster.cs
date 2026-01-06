using System;
using UnityEngine;

namespace Blade.Combat
{
    public class SphereDamageCaster : DamageCaster
    {
        [SerializeField, Range(0.5f, 3f)] private float castRadius = 1f;
        [SerializeField, Range(0, 1f)] private float casterInterpolation = 0.5f;
        [SerializeField, Range(0, 3f)] private float castingRange = 1f;
        
        public override bool CastDamage(DamageData damageData, Vector3 position, Vector3 direction, AttackDataSO attackData)
        {
            Vector3 startPosition = position + direction * -casterInterpolation * 2; // -붙어있음.

            bool isHit = Physics.SphereCast(
                startPosition, 
                castRadius, 
                transform.forward, 
                out RaycastHit hit,
                castingRange, 
                whatIsTarget);

            if (isHit)
            {
                Debug.Log($"<color=red>Hit!</color>");
                ApplyDamageAndKnockBack(hit.collider.transform, damageData,
                    hit.point, hit.normal, attackData);
            }

            return isHit;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Vector3 startPosition = transform.position + transform.forward * -casterInterpolation * 2;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPosition, castRadius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPosition + transform.forward * castingRange, castRadius);
        }
#endif
    }
}