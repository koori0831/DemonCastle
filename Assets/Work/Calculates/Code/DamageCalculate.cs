using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;
using Work.Entities;

namespace Work.Calculates.Code
{
    public class DamageCalculate //데미지 계산해주는 클래스 
    {
        private Entity _owner;
        private DetectSensorCompo _dectectSensor;
        private StatContainer _statContainer;
        private const float C = 0.2f;

        public DamageCalculate(Entity owner)
        {
            _owner = owner;
            _dectectSensor = _owner.GetCompo<DetectSensorCompo>();
            _statContainer = _owner.StatContainer;
        }

        public float CalculateDamage(AttackTypeEnum attackType,float damage)
        {
            if (_dectectSensor.CurrentTarget != null) return damage;
            Entity target = _dectectSensor.CurrentTarget.Transform.GetComponent<Entity>();
            StatContainer targetStat = target.StatContainer;

            Stat defenceStat = targetStat.GetDefenceStatForAttackType(attackType);
            Stat attackStat = _statContainer.GetAttackStatForAttackType(attackType);
            Debug.Assert(defenceStat != null, $"not found defenceStat");
            Debug.Assert(attackStat != null, $"not found attackStat");
            float defence = defenceStat.GetStatValue();
            float attack = attackStat.GetStatValue();

            float damageReduction = (C * defence) / (attack + C * defence); //경감률

            return damage * damageReduction;

        }
    }
}