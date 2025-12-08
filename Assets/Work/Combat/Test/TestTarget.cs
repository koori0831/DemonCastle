using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Combat.Test
{
    public class TestTarget : MonoBehaviour, IDamageable
    {
        public Rigidbody rbCompo;

        public Transform Transform => this != null ? transform : null;

        public Action<IDamageable> OnDeadEvent { get; set; }
        public bool IsDead { get; private set; }

        public float health = 100;

        public void TakeDamage(Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0)
        {
            health -= damage;

            if(isKnockback)
            {
                rbCompo.AddForce(-normal * knockbackPower);
            }

            if(health <= 0)
            {
                IsDead = true;
                OnDeadEvent?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}
