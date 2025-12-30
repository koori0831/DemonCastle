using Blade.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "RotateToTarget", story: "[Self] rotate to [Target] with [Movement]", category: "Enemy/Move", id: "cf3c71ccd199afb722181870ff469662")]
public partial class RotateToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Self;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<NavMovement> Movement;

    [SerializeReference] public BlackboardVariable<float> RotationThreshold;
    
    protected override Status OnUpdate()
    {
        if (LookTargetSmoothly())
        {
            return Status.Success;
        }
        return Status.Running;
    }

    private bool LookTargetSmoothly()
    {
        Quaternion targetRotation = Movement.Value.LookAtTarget(Target.Value.position);
        return Quaternion.Angle(targetRotation, Self.Value.transform.rotation) < RotationThreshold.Value;
    }
}

