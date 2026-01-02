using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WaitForAnimation", story: "Wait for [Trigger] end", category: "Enemy/Animation", id: "3af1ac4b0f6872a173aef3c99952075f")]
    public partial class WaitForAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimatorTrigger> Trigger;

        private bool _isTriggered = false;
        
        protected override Status OnStart()
        {
            _isTriggered = false;
            Trigger.Value.OnAnimationEndTrigger += HandleAnimationEndTrigger;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            return  _isTriggered ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            Trigger.Value.OnAnimationEndTrigger -= HandleAnimationEndTrigger;
        }

        private void HandleAnimationEndTrigger()
        {
            _isTriggered = true;
        }
    }
}

