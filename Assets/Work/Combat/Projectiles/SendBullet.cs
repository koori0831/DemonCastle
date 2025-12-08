using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Combat.Projectiles
{
    public class SendBullet : Projectile
    {
        [SerializeField] private ParticleSystem destroyEffect;
        private float _damage;

        protected override void OnCollisionAfter(Collision collision)
        {
            if(collision.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                Vector3 normal = transform.position - collision.collider.ClosestPoint(transform.position);

                damageable.TakeDamage(_owner,_damage, normal.normalized, true, 2);
            }

            Instantiate(destroyEffect,transform.position,Quaternion.identity);
            base.OnCollisionAfter(collision);
        }

        public void SetDamage(float value) => _damage = value;
    }
}
