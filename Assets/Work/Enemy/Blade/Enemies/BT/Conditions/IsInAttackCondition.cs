using System;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "IsInAttack", story: "[Self] check in attack from [Target]", category: "Enemy/Condition", id: "cf49a5c781c506d29f900b65ace09df7")]
    public partial class IsInAttackCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        public override bool IsTrue()
        {
            float distance = Vector3.Distance(Self.Value.transform.position, Target.Value.position);
             
            return distance < Self.Value.attackRange;
        }
    }
}
