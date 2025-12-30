using System;
using System.Collections.Generic;
using System.Linq;
using Blade.Core;
using Blade.Events;
using Blade.SkillSystem.Upgrade;
using TMPro;
using UnityEngine;

namespace Blade.UI.TabMenu
{
    public class SkillTreePanelUI : AbstractTabPanelUI
    {
        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }
        [SerializeField] private TextMeshProUGUI skillPointText;
        
        private Dictionary<SkillUpgradeSO, SkillUpgradeUI> _skillUIDict;

        private void Awake()
        {
            PlayerChannel.AddListener<SkillTreeUpdateEvent>(HandleSkillTreeUpdate);
            _skillUIDict = GetComponentsInChildren<SkillUpgradeUI>()
                            .ToDictionary(ui => ui.upgradeData);
        }

        private void OnDestroy()
        {
            PlayerChannel.RemoveListener<SkillTreeUpdateEvent>(HandleSkillTreeUpdate);
        }

        private void HandleSkillTreeUpdate(SkillTreeUpdateEvent evt)
        {
            foreach (var skill in evt.skills.Values)
            {
                foreach (var upgradeData in skill.SkillData.upgradeList)
                {
                    if (_skillUIDict.TryGetValue(upgradeData, out var skillUI))
                    {
                        int cnt = skill.GetUpgradeCount(upgradeData); //이 업그레이드가 몇개나 진행되었는지
                        
                        skillUI.SetUnlock(cnt > 0);
                        skillUI.UpdateUpgradeText(cnt);
                        skillUI.SetTargetSkill(skill);
                    }
                }
            }
            
            skillPointText.SetText(evt.skillPoint.ToString());
        }

        public override void OpenPanel(bool isTween)
        {
            base.OpenPanel(isTween);
            foreach (SkillUpgradeUI ui in _skillUIDict.Values)
            {
                ui.UpgradeBtn.onClick.AddListener(() =>
                {
                    if (ui.upgradeData == null) return;
                    PlayerChannel.RaiseEvent(
                        PlayerEvents.SkillUpgradeEvent.Initializer(ui.TargetSkill, ui.upgradeData));
                });
            }
        }

        public override void ClosePanel(bool isTween)
        {
            foreach (SkillUpgradeUI ui in _skillUIDict.Values)
            {
                ui.UpgradeBtn.onClick.RemoveAllListeners();
            }
            base.ClosePanel(isTween);
        }
    }
}