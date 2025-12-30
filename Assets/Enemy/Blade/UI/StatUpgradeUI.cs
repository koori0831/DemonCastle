using System;
using Blade.StatSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blade.UI
{
    public class StatUpgradeUI : MonoBehaviour
    {
        [field: SerializeField] public StatSO StatData { get; private set; }

        [SerializeField] private Image statIcon;
        [SerializeField] private TextMeshProUGUI statNameText;

        [SerializeField] private Button minusButton;
        [SerializeField] private TextMeshProUGUI statValueText;
        [SerializeField] private Button plusButton;

        public event Action<StatSO> OnPlusButtonClicked;
        public event Action<StatSO> OnMinusButtonClicked;
        
        public string StatValue
        {
            get => statValueText.text;
            set => statValueText.SetText(value);
        }

        private void Awake()
        {
            plusButton.onClick.AddListener(() => OnPlusButtonClicked?.Invoke(StatData));
            minusButton.onClick.AddListener(() => OnMinusButtonClicked?.Invoke(StatData));
        }


        public void SetPlusButton(bool isActive, bool onlyImage = false)
        {
            if(onlyImage)
                plusButton.image.enabled = isActive;
            else
                plusButton.gameObject.SetActive(isActive);
        }
        
        public void SetMinusButton(bool isActive, bool onlyImage = false)
        {
            if(onlyImage)
                minusButton.image.enabled = isActive;
            else
                minusButton.gameObject.SetActive(isActive);
        }

        private void OnValidate()
        {
            if (StatData == null) return;
            
            if(statIcon != null)
                statIcon.sprite = StatData.icon;
            if(statNameText != null)
                statNameText.SetText(StatData.displayName);
            
            gameObject.name = $"StatUpgradeUI_{StatData.statName}";
        }
    }
}