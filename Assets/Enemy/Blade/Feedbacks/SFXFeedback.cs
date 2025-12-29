using Blade.Core;
using Blade.Events;
using Blade.SoundSystem;
using UnityEngine;

namespace Blade.Feedbacks
{
    public class SFXFeedback : Feedback
    {
        [SerializeField] private GameEventChannelSO soundChannel;
        [SerializeField] private SoundSO feedbackSound;
        public override void CreateFeedback()
        {
            var soundEvt = SoundEvents.PlaySFXEvent.Initialize(transform.position, feedbackSound);
            soundChannel.RaiseEvent(soundEvt);
        }

        public override void StopFeedback()
        {
        }
    }
}