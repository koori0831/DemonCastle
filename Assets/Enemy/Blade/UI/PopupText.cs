using DG.Tweening;
using GondrLib.ObjectPool.RunTime;
using TMPro;
using UnityEngine;

namespace Blade.UI
{
    public class PopupText : MonoBehaviour, IPoolable
    {
        [SerializeField] private TextMeshPro popupText;
        
        private Pool _pool;
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        
        public void SetUpPool(Pool pool) => _pool = pool;

        public void ResetItem()
        {
            transform.localScale = Vector3.one;
            popupText.alpha = 1f;
        }

        private void LateUpdate()
        {
            Transform mainCamera = Camera.main.transform;
            Vector3 cameraDirection = (transform.position - mainCamera.position).normalized;
            transform.forward = cameraDirection;
        }

        public void ShowPopupText(string text, TextInfoSO textInfo, Vector3 position, float duration)
        {
            popupText.SetText(text);
            popupText.color = textInfo.textColor;
            popupText.fontSize = textInfo.fontSize;
            transform.position = position;

            float scaleTime = 0.2f;
            float fadeTime = 1.2f;
            
            Sequence seq = DOTween.Sequence();
            seq.Append(transform.DOScale(2.5f, scaleTime));
            seq.Append(transform.DOScale(1.2f, scaleTime));
            seq.AppendInterval(duration);
            seq.Append(transform.DOScale(0.3f, fadeTime));
            seq.Join(popupText.DOFade(0, fadeTime));
            seq.Join(transform.DOMoveY(position.y + 2f, fadeTime));
            seq.AppendCallback(() => _pool.Push(this));
        }
    }
}