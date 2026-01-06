using System;
using System.Threading;
using LitMotion;
using LitMotion.Extensions;
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

        // 수동 취소(StopFeedback)를 위한 CancellationTokenSource
        private CancellationTokenSource _cts;

        public UnityEvent FeedbackComplete;

        private void OnDestroy()
        {
            StopFeedback();
        }

        public override async void CreateFeedback()
        {
            if (_isAlreadyStart) return;
            _isAlreadyStart = true;

            // 기존 작업이 있다면 정리하고 새로 토큰 생성
            StopFeedbackInternal();
            _cts = new CancellationTokenSource();

            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                _cts.Token,
                this.destroyCancellationToken
            );

            try
            {
                // 1. 초기 딜레이 (Awaitable 사용)
                await Awaitable.WaitForSecondsAsync(delayTime, linkedCts.Token);

                // 2. 이펙트 재생
                if (feedbackEffect != null) feedbackEffect.Play();

                // 3. 머티리얼 애니메이션 (LitMotion + Awaitable)
                float startValue = meshRenderer.material.GetFloat(_dissolveHeight);
                float endValue = -2f;

                await LMotion.Create(startValue, endValue, 0.8f)
                    .WithEase(Ease.OutQuad)
                    .BindToMaterialFloat(meshRenderer.material, _dissolveHeight)
                    .ToAwaitable(linkedCts.Token); // Awaitable로 변환하여 대기

                // 4. 이펙트 재생 완료 대기 (2초)
                await Awaitable.WaitForSecondsAsync(2f, linkedCts.Token);

                // 5. 완료 이벤트 호출
                FeedbackComplete?.Invoke();
            }
            catch (OperationCanceledException)
            {
                // 취소되었을 때 발생하는 예외 (무시해도 됨)
            }
            catch (Exception e)
            {
                Debug.LogError($"[DisintegrateFeedback] Error: {e}");
            }
            finally
            {
                if (_cts != null && !_cts.IsCancellationRequested)
                {
                    _isAlreadyStart = false;
                    linkedCts.Dispose();
                }
            }
        }

        public override void StopFeedback()
        {
            StopFeedbackInternal();
            _isAlreadyStart = false;
        }

        private void StopFeedbackInternal()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }
        }
    }
}