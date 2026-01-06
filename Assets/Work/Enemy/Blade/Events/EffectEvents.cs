using Blade.Core;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.Events
{
    public static class EffectEvents
    {
        public static PlayPoolEffect PlayPoolEffect = new PlayPoolEffect();
        public static PopupTextEvent PopupTextEvent = new PopupTextEvent();
    }

    public class PlayPoolEffect : GameEvent
    {
        public Vector3 position;
        public Quaternion rotation;
        public PoolItemSO effectItem;
        public float duration;

        public PlayPoolEffect Initializer(Vector3 position, Quaternion rotation, PoolItemSO item, float duration)
        {
            this.position = position;
            this.rotation = rotation;
            this.effectItem = item;
            this.duration = duration;
            
            return this;
        }
    }

    public class PopupTextEvent : GameEvent
    {
        public string textMsg;
        public int textTypeHash;
        public Vector3 position;
        public float showDuration;
        
        public PopupTextEvent Initialize(string textMsg, int textTypeHash, Vector3 position, float showDuration)
        {
            this.textMsg = textMsg;
            this.textTypeHash = textTypeHash;
            this.position = position;
            this.showDuration = showDuration;
            return this;
        }
    }
}