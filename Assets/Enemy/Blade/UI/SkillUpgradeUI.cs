using System;
using Blade.SkillSystem;
using Blade.SkillSystem.Upgrade;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Blade.UI
{
    public class SkillUpgradeUI : MonoBehaviour
    {
        [field: SerializeField] public Button UpgradeBtn { get; private set; }

        [SerializeField] private Image upgradeImage;
        [SerializeField] private TextMeshProUGUI upgradeText;
        [SerializeField] private GameObject lockImage;

        public SkillUpgradeSO upgradeData;
        
        public Skill TargetSkill { get; private set; }
        public RectTransform RectTrm { get; private set; }

        private void Awake()
        {
            RectTrm = GetComponent<RectTransform>();
        }
        
        public void SetTargetSkill(Skill skill) => TargetSkill = skill;
        public void SetUnlock(bool isUnlock) => lockImage.SetActive(!isUnlock);

        public void UpdateUpgradeText(int count)
        {
            if(upgradeData.maxUpgradeCount > 1)
                upgradeText.SetText($"{count}/{upgradeData.maxUpgradeCount}");
            else
                upgradeText.SetText(string.Empty);
        }

        private void OnValidate()
        {
            if (upgradeData == null) return;
            
            gameObject.name = $"SkillUpgradeBtn_{upgradeData.upgradeTitle}";
            if (upgradeImage != null)
                upgradeImage.sprite = upgradeData.upgradeIcon;
            if (upgradeText != null)
                UpdateUpgradeText(0);
        }
    }
}