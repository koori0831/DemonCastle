using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Set LookAtTarget", story: "Set Look at [Target] to [NavMovement]", category: "Action", id: "cc18b84b3f885ee0fda95bb24022b27f")]
    public partial class SetLookAtTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<NavMovement> NavMovement;

        protected override Status OnStart()
        {
            NavMovement.Value.SetLookAtTarget(Target.Value);
            return Status.Success;
        }
    }
}

