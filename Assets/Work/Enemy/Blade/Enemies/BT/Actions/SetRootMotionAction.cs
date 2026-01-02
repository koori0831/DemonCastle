using System;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetRootMotion", story: "Set RootMotion [MainAnimator] [NewValue]", category: "Action", id: "76e8ccc05e783291c38dc36f4a80f92c")]
    public partial class SetRootMotionAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityAnimator> MainAnimator;
        [SerializeReference] public BlackboardVariable<bool> NewValue;

        protected override Status OnStart()
        {
            MainAnimator.Value.ApplyRootMotion = NewValue;
            return Status.Success;
        }
    }
}

