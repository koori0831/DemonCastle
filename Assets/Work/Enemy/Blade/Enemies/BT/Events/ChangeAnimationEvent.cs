using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

namespace Blade.Enemies.BT.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/ChangeAnimationEvent")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "ChangeAnimationEvent", message: "change to [newClip]", category: "Events", id: "60136445aafc8bfa31e9b4447f449575")]
    public partial class ChangeAnimationEvent : EventChannelBase
    {
        public delegate void ChangeAnimationEventEventHandler(string newClip);
        public event ChangeAnimationEventEventHandler Event; 

        public void SendEventMessage(string newClip)
        {
            Event?.Invoke(newClip);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<string> newClipBlackboardVariable = messageData[0] as BlackboardVariable<string>;
            var newClip = newClipBlackboardVariable != null ? newClipBlackboardVariable.Value : default(string);

            Event?.Invoke(newClip);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            ChangeAnimationEventEventHandler del = (newClip) =>
            {
                BlackboardVariable<string> var0 = vars[0] as BlackboardVariable<string>;
                if(var0 != null)
                    var0.Value = newClip;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as ChangeAnimationEventEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as ChangeAnimationEventEventHandler;
        }
    }
}

