using System;
using System.Collections.Generic;
using System.Linq;
using Blade.Core;
using Blade.Events;
using Blade.StatSystem;
using TMPro;
using UnityEngine;

namespace Blade.UI.TabMenu
{
    public class StatPanelUI : AbstractTabPanelUI
    {
        [SerializeField] private GameEventChannelSO playerChannel;
        [SerializeField] private TextMeshProUGUI statPointText;
        
        private Dictionary<StatSO, StatUpgradeUI> _statUIDict;

        private void Awake()
        {
            _statUIDict = GetComponentsInChildren<StatUpgradeUI>()
                .ToDictionary(ui => ui.StatData);
        }

        public override void OpenPanel(bool isTween)
        {
            base.OpenPanel(isTween);
            playerChannel.AddListener<ResponseStat>(HandleResponseStat);
            playerChannel.RaiseEvent(PlayerEvents.RequestStat);

            if (_statUIDict != null)
            {
                foreach (var ui in _statUIDict.Values)
                {
                    ui.OnPlusButtonClicked += HandleStatIncrement;
                    ui.OnMinusButtonClicked += HandleStatDecrement;
                }
            }
        }

        public override void ClosePanel(bool isTween)
        {
            playerChannel.RemoveListener<ResponseStat>(HandleResponseStat);

            if (_statUIDict != null)
            {
                foreach (var ui in _statUIDict.Values)
                {
                    ui.OnPlusButtonClicked -= HandleStatIncrement;
                    ui.OnMinusButtonClicked -= HandleStatDecrement;
                }
            }
            base.ClosePanel(isTween);
        }

        private void HandleStatIncrement(StatSO target)
        {
            playerChannel.RaiseEvent(PlayerEvents.ChangeStatEvent.Initializer(target));
        }

        private void HandleStatDecrement(StatSO target)
        {
            playerChannel.RaiseEvent(PlayerEvents.ChangeStatEvent.Initializer(target, -1));
        }

        private void HandleResponseStat(ResponseStat evt)
        {
            EntityStatCompo statCompo = evt.statCompo;
            int statPoint = evt.statPoint;
            statPointText.SetText(statPoint.ToString());

            foreach (var kvp in _statUIDict)
            {
                StatSO stat = kvp.Key;
                StatUpgradeUI statUI = kvp.Value;
                StatSO realStat = statCompo.GetStat(stat);
                float statValue = realStat.Value;
                statUI.StatValue = stat.IsPercent 
                    ? $"{Mathf.Round(statValue * 100)}%" : 
                    (Mathf.Round(statValue * 100f) / 100f).ToString();
                
                statUI.SetMinusButton(false); //마이너스는 안쓸꺼니까 일단 끈다.
                statUI.SetPlusButton(statPoint > 0 && realStat.CanIncrementStep(), true);
            }
        }
    }
}