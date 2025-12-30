using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

namespace Blade.Feedbacks
{
    public class DisintegrateFeedback : Feedback
    {
        [SerializeField] private float delayTime = 3f;
        [SerializeField] private VisualEffect feedbackEffect;
        [SerializeField] private SkinnedMeshRenderer meshRenderer;

        private bool _isAlreadyStart = false;
        private readonly int _dissolveHeight = Shader.PropertyToID("_DissolveHeight");

        public UnityEvent FeedbackComplete;
        
        public override void CreateFeedback()
        {
            if (_isAlreadyStart) return;

            _isAlreadyStart = true;

            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(delayTime); //딜레이만큼 기다렸다가
            seq.AppendCallback(() => feedbackEffect.Play()); //이펙트 재생
            seq.Append(meshRenderer.material.DOFloat(-2f, _dissolveHeight, 
                0.8f));
            seq.AppendInterval(2f); //이펙트 재생 완료시까지 대기
            seq.OnComplete(() => FeedbackComplete?.Invoke());

        }

        public override void StopFeedback()
        {
            
        }
    }
}