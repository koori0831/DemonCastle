using System;
using UnityEngine;

namespace Blade.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        public DamageType damageType = DamageType.MELEE;
        
        public string attackName;
        public MovementDataSO movementData;
        public float damageMultiplier = 1f;  // 증뎀 - 곱연산
        public float damageIncrease = 0; // 추뎀 - 합연산
        public bool isPowerAttack;
        public float impulseForce;
        // public float knockBackForce;
        // public float knockBackDuration;
        public MovementDataSO knockBackMovement;

        private void OnEnable()
        {
            attackName = this.name;
        }
    }
}