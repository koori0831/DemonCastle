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
        public Transform Transform => this != null ? transform : null;

        public void TakeDamage(Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0)
        {
            Debug.Log("맞는중");
        }
    }
}
