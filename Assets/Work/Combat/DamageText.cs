using System.Collections;
using Blade.UI;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace Work.Combat
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro popupText;

        private CompositeMotionHandle _handles;
        private Coroutine _routine;

        private void Awake()
        {
            _handles = new CompositeMotionHandle(8);
        }

        private void OnDisable()
        {
            // 오브젝트가 꺼지거나 파괴될 때 남은 모션 정리
            _handles.Cancel();
            if (_routine != null) StopCoroutine(_routine);
            _routine = null;
        }

        private void LateUpdate()
        {
            var cam = Camera.main;
            if (!cam) return;

            Transform mainCamera = cam.transform;
            Vector3 cameraDirection = (transform.position - mainCamera.position).normalized;
            transform.forward = cameraDirection;
        }

        public void Init(string text, TextInfoSO textInfo, Vector3 position, float duration)
        {
            popupText.SetText(text);
            popupText.color = textInfo.textColor;
            popupText.fontSize = textInfo.fontSize;
            transform.position = position;

            // 재사용/연타 호출 대비: 이전 모션/코루틴 정리
            _handles.Cancel();
            if (_routine != null) StopCoroutine(_routine);

            // DOTween ResetItem 같은 기본값
            transform.localScale = Vector3.one;
            popupText.alpha = 1f;

            _routine = StartCoroutine(PlayRoutine(position, duration));
        }

        private IEnumerator PlayRoutine(Vector3 startPos, float duration)
        {
            const float scaleTime = 0.2f;
            const float fadeTime = 1.2f;

            // 1) scale up
            var h1 = LMotion.Create(Vector3.one, Vector3.one * 2.5f, scaleTime)
                .WithEase(Ease.OutQuad)
                .BindToLocalScale(transform)
                .AddTo(gameObject);
            _handles.Add(h1);
            yield return h1.ToYieldInstruction();

            // 2) scale down
            var h2 = LMotion.Create(Vector3.one * 2.5f, Vector3.one * 1.2f, scaleTime)
                .WithEase(Ease.OutQuad)
                .BindToLocalScale(transform)
                .AddTo(gameObject);
            _handles.Add(h2);
            yield return h2.ToYieldInstruction();

            // 3) interval
            if (duration > 0f)
                yield return new WaitForSeconds(duration);

            // 4) fade group (동시에)
            var hScale = LMotion.Create(Vector3.one * 1.2f, Vector3.one * 0.3f, fadeTime)
                .WithEase(Ease.InQuad)
                .BindToLocalScale(transform)
                .AddTo(gameObject);

            var hAlpha = LMotion.Create(1f, 0f, fadeTime)
                .WithEase(Ease.InQuad)
                .Bind(popupText, static (a, t) => t.alpha = a)
                .AddTo(gameObject);

            var hMoveY = LMotion.Create(startPos.y, startPos.y + 2f, fadeTime)
                .WithEase(Ease.OutQuad)
                .Bind((this, startPos), static (y, state) =>
                {
                    var (self, pos) = state;
                    var p = self.transform.position;
                    p.y = y;
                    self.transform.position = p;
                })
                .AddTo(gameObject);

            _handles.Add(hScale);
            _handles.Add(hAlpha);
            _handles.Add(hMoveY);

            // fadeTime 동안만 기다리면 셋 다 끝남(동일 duration)
            yield return hScale.ToYieldInstruction();

            // 시퀀스/콜백 중간에 Destroy하지 말고, “완료 후” 여기서 파괴
            Destroy(gameObject);
        }
    }
}
