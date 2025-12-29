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
        private bool _isKnockback = false;
        private float _timer;
        private const float DELAY_TIME = 0.1f;
        private void Update()
        {
            if(_isKnockback)
            {
                _timer += Time.deltaTime;
                if(_timer > DELAY_TIME)
                {
                    _isKnockback = false;
                    _timer = 0; 
                }
            }
        }

        public void TakeDamage(Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0)
        {
            health -= damage;

            if(isKnockback && !_isKnockback)
            {
                Debug.Log("Knockback");
                _isKnockback = true;
                rbCompo.AddForce(normal * knockbackPower,ForceMode.Impulse);
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
