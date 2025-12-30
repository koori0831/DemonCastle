using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blade.UI.TabMenu
{
    public class TabButtonUI : MonoBehaviour
    {
        [SerializeField] private Button tabButton;
        [SerializeField] private Image selectBox;
        [SerializeField] private TextMeshProUGUI tabText;
        [field: SerializeField] public TabDataSO TabData { get; private set; }
        
        public event Action<TabDataSO> OnTabButtonClicked;

        private void OnValidate()
        {
            if (TabData == null) return;
            gameObject.name = $"TabButtonUI_{TabData.TabName}";

            if (tabText != null)
            {
                tabText.SetText(TabData.TabButtonText);
            }
        }
        
        private void Awake()
        {
            tabButton.onClick.AddListener(HandleButtonClick);
        }

        private void OnDestroy()
        {
            tabButton.onClick.RemoveListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            OnTabButtonClicked?.Invoke(TabData);
        }

        public void SetActive(bool isActive)
        {
            float targetScale = isActive ? 1 : 0;
            selectBox.transform.DOKill();
            selectBox.transform.DOScaleX(targetScale, 0.25f).SetUpdate(true);
        }

        
    }
}