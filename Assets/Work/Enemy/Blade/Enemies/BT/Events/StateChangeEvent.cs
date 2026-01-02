using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Blade.Enemies.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/StateChangeEvent")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "StateChangeEvent", message: "Set [nextState]", category: "Events", id: "869f51dc1049e0cfc48922cc43e4fb0a")]
    public partial class StateChangeEvent : EventChannelBase
    {
        public delegate void StateChangeEventEventHandler(EnemyState nextState);
        public event StateChangeEventEventHandler Event; 

        public void SendEventMessage(EnemyState nextState)
        {
            Event?.Invoke(nextState);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<EnemyState> nextStateBlackboardVariable = messageData[0] as BlackboardVariable<EnemyState>;
            var nextState = nextStateBlackboardVariable != null ? nextStateBlackboardVariable.Value : default(EnemyState);

            Event?.Invoke(nextState);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            StateChangeEventEventHandler del = (nextState) =>
            {
                BlackboardVariable<EnemyState> var0 = vars[0] as BlackboardVariable<EnemyState>;
                if(var0 != null)
                    var0.Value = nextState;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as StateChangeEventEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as StateChangeEventEventHandler;
        }
    }
}

