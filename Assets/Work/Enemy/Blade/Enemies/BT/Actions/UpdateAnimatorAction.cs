using Blade.Enemies;
using Blade.Entities;
using System;
using Blade.Enemies.Skeleton;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateAnimator", story: "[Self] update [MainAnimator] from [Movement]", category: "Action", id: "cc809115ac2d73b76141c5516c962741")]
public partial class UpdateAnimatorAction : Action
{
    [SerializeReference] public BlackboardVariable<CommonEnemy> Self;
    [SerializeReference] public BlackboardVariable<EntityAnimator> MainAnimator;
    [SerializeReference] public BlackboardVariable<NavMovement> Movement;
    
    private readonly int _xMoveHash = Animator.StringToHash("X_MOVE");
    private readonly int _zMoveHash = Animator.StringToHash("Z_MOVE");

    private Transform _selfTrm;
    
    protected override Status OnStart()
    {
        _selfTrm = Self.Value.transform;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 velocity = Movement.Value.Velocity; //이거 만들어주러 간다.
        float xMove = Vector3.Dot(_selfTrm.right, velocity);
        float zMove = Vector3.Dot(_selfTrm.forward, velocity);
        
        MainAnimator.Value.SetParam(_xMoveHash, xMove, 0.15f);
        MainAnimator.Value.SetParam(_zMoveHash, zMove, 0.15f);
        
        return Status.Running;
    }
    
}

