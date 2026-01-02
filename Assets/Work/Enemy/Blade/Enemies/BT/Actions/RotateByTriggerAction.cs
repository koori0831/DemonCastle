using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "RotateByTrigger", story: "[Movement] rotate to [Target] by [Trigger]", category: "Enemy/Move", id: "562fb41d3b155cdf8260d59a373a59f3")]
    public partial class RotateByTriggerAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> Movement;
        [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        private bool _isRotate = false;
        
        protected override Status OnStart()
        {
            Trigger.Value.OnManualRotationTrigger += HandleManualRotation;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_isRotate)
            {
                Movement.Value.LookAtTarget(Target.Value.position);
            }
            return Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnManualRotationTrigger -= HandleManualRotation;
        }

        private void HandleManualRotation(bool isRotate) => _isRotate = isRotate;
    }
}

