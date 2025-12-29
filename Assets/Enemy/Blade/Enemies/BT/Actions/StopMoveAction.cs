using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "StopMove", story: "[Movement] isStop set to [newValue]", category: "Enemy/Move", id: "e8de670b595a7e067ec9c43d0947d511")]
    public partial class StopMoveAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            Movement.Value.SetStop(NewValue.Value);
            if (NewValue.Value)
            {
                Movement.Value.SetDestination(Movement.Value.transform.position);
            }
            return Status.Success;
        }
    }
}

