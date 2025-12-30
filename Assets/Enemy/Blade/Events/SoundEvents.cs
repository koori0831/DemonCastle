using Blade.Core;
using Blade.SoundSystem;
using UnityEngine;

namespace Blade.Events
{
    public static class SoundEvents
    {
        public static PlaySFXEvent PlaySFXEvent = new PlaySFXEvent();
        // public static PlayBGMEvent PlayBGMEvent = new PlayBGMEvent();
        public static StopSoundEvent StopSoundEvent = new StopSoundEvent();
    }

    public class PlaySFXEvent : GameEvent
    {
        public Vector3 position;
        public SoundSO soundClip;
        public int channel;

        public PlaySFXEvent Initialize(Vector3 position, SoundSO soundClip, int channel = 0)
        {
            this.position = position;
            this.soundClip = soundClip;
            this.channel = channel;
            return this;
        }
    }

    public class StopSoundEvent : GameEvent
    {
        public int channel;

        public StopSoundEvent Initialize(int channel)
        {
            this.channel = channel;
            return this;
        }
    }
}