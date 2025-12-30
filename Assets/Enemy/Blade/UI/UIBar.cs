using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Blade.UI
{
    public class UIBar : MonoBehaviour
    {
        public enum UIBarType
        {
            HP, EXP
        }
        
        [SerializeField] private Transform barTrm;
        [SerializeField] private TextMeshProUGUI barText;
        [field: SerializeField] public UIBarType BarType { get; private set; }

        public void SetText(string text)
        {
            barText.SetText(text);
        }

        public void SetNormalizedValue(float normalizedValue)
        {
            barTrm.DOKill();
            barTrm.DOScaleX(normalizedValue, 0.05f);
        }
    }
}