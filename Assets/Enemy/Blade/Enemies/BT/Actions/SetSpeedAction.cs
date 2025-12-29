using Blade.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetSpeed", story: "Set Speed [NavMovement] to [NewValue]", category: "Action", id: "13433026abe73e58743eea13de9c20b7")]
public partial class SetSpeedAction : Action
{
    [SerializeReference] public BlackboardVariable<NavMovement> NavMovement;
    [SerializeReference] public BlackboardVariable<float> NewValue;

    protected override Status OnStart()
    {
        if (NavMovement.Value == null)
            return Status.Failure;
        
        NavMovement.Value.SpeedMultiplier = NewValue.Value;
        return Status.Success;
    }
}

