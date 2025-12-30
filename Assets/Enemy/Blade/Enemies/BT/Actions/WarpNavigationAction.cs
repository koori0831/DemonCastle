using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "WarpNavigation", story: "[NavMovement] Warp to [Self]", category: "Action", id: "ffcd5f07a7449439756808ef97d5069d")]
    public partial class WarpNavigationAction : Action
    {
        [SerializeReference] public BlackboardVariable<NavMovement> NavMovement;
        [SerializeReference] public BlackboardVariable<Transform> Self;

        protected override Status OnStart()
        {
            NavMovement.Value.WarpToPosition(Self.Value.position);
            return Status.Success;
        }
    }
}

