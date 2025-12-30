using Blade.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Work.Combat
{
    public class DamageText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro popupText;


        private void LateUpdate()
        {
            Transform mainCamera = Camera.main.transform;
            Vector3 cameraDirection = (transform.position - mainCamera.position).normalized;
            transform.forward = cameraDirection;
        }

        public void Init(string text, TextInfoSO textInfo, Vector3 position, float duration)
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
            seq.AppendCallback(() => Destroy(gameObject));
        }
    }
}