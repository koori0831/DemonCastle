using System.Collections;
using GondrLib.ObjectPool.RunTime;
using LitMotion;
using LitMotion.Extensions;
using TMPro;
using UnityEngine;

namespace Blade.UI
{
    public class PopupText : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshPro popupText;

        private Pool _pool;
        private CompositeMotionHandle _handles;
        private Coroutine _routine;

        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            _handles = new CompositeMotionHandle(8);
        }

        public void SetUpPool(Pool pool) => _pool = pool;

        public void ResetItem()
        {
            _handles.Cancel();
            if (_routine != null) StopCoroutine(_routine);
            _routine = null;

            transform.localScale = Vector3.one;
            popupText.alpha = 1f;
        }

        private void OnDisable()
        {
            // 풀로 돌아가거나 꺼질 때 잔여 모션 정리
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

        public void ShowPopupText(string text, TextInfoSO textInfo, Vector3 position, float duration)
        {
            popupText.SetText(text);
            popupText.color = textInfo.textColor;
            popupText.fontSize = textInfo.fontSize;
            transform.position = position;

            ResetItem(); // 기본값 + 이전 모션 정리

            _routine = StartCoroutine(PlayRoutine(position, duration));
        }

        private IEnumerator PlayRoutine(Vector3 startPos, float duration)
        {
            const float scaleTime = 0.2f;
            const float fadeTime = 1.2f;

            var h1 = LMotion.Create(Vector3.one, Vector3.one * 2.5f, scaleTime)
                .WithEase(Ease.OutQuad)
                .BindToLocalScale(transform)
                .AddTo(gameObject);
            _handles.Add(h1);
            yield return h1.ToYieldInstruction();

            var h2 = LMotion.Create(Vector3.one * 2.5f, Vector3.one * 1.2f, scaleTime)
                .WithEase(Ease.OutQuad)
                .BindToLocalScale(transform)
                .AddTo(gameObject);
            _handles.Add(h2);
            yield return h2.ToYieldInstruction();

            if (duration > 0f)
                yield return new WaitForSeconds(duration);

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

            yield return hScale.ToYieldInstruction();

            // 완전히 끝난 뒤 풀 반환
            _pool.Push(this);
        }
    }
}
