using LitMotion;
using UnityEngine;

namespace Blade.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private SkinnedMeshRenderer meshRenderer;
        [SerializeField] private float blinkDuration = 0.15f;
        [SerializeField] private float blinkIntensity = 0.25f;

        private readonly int _blinkHash = Shader.PropertyToID("_BlinkValue");

        public override void CreateFeedback()
        {
            meshRenderer.material.SetFloat(_blinkHash, blinkIntensity);
            //이건 개선할 수 있어.
            /*DOVirtual.DelayedCall(blinkDuration, () =>
            {
                meshRenderer.material.SetFloat(_blinkHash, 0f);
            });*/
            LMotion.Create(0f, 0f, 0f) // 즉시 끝나는 모션 생성
            .WithDelay(blinkDuration) // 실행 전 대기 시간 설정
            .WithOnComplete(() =>
                {
                    meshRenderer.material.SetFloat(_blinkHash, 0f);
                })
            .RunWithoutBinding();
        }

        public override void StopFeedback()
        {
            if (meshRenderer != null)
            {
                meshRenderer.material.SetFloat(_blinkHash, 0f);
            }
        }
    }
}