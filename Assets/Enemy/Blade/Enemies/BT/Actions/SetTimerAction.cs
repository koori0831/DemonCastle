using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SetTimer", story: "Set time to [Timer]", category: "Action", id: "777a21ed1aa4d68cd1a2b3c8f81a5b8d")]
    public partial class SetTimerAction : Action
    {
        [SerializeReference] public BlackboardVariable<float> Timer;

        protected override Status OnStart()
        {
            Timer.Value = Time.time;
            return Status.Success;
        }
    }
}

