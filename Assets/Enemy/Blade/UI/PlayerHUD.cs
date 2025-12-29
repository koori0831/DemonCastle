using System;
using System.Collections.Generic;
using System.Linq;
using Blade.Core;
using Blade.Events;
using UnityEngine;

namespace Blade.UI
{
    public class PlayerHUD : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO playerChannel;

        private Dictionary<UIBar.UIBarType, UIBar> _bars;

        private void Awake()
        {
            _bars = GetComponentsInChildren<UIBar>().ToDictionary(bar => bar.BarType);
            
            playerChannel.AddListener<PlayerHealthEvent>(HandleHealthChange);
            playerChannel.AddListener<PlayerExpEvent>(HandleExpChange);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<PlayerHealthEvent>(HandleHealthChange);
            playerChannel.RemoveListener<PlayerExpEvent>(HandleExpChange);
        }

        private void HandleHealthChange(PlayerHealthEvent evt)
        {
            UIBar targetBar = _bars.GetValueOrDefault(UIBar.UIBarType.HP);
            SetTargetBar(targetBar, evt.health, evt.maxHealth);
        }

        private void HandleExpChange(PlayerExpEvent evt)
        {
            UIBar targetBar = _bars.GetValueOrDefault(UIBar.UIBarType.EXP);
            SetTargetBar(targetBar, evt.currentExp, evt.maxExp);
        }
        
        private void SetTargetBar(UIBar targetBar, float value, float maxValue)
        {
            targetBar.SetText($"{value} / {maxValue}");
            targetBar.SetNormalizedValue(value / maxValue);
        }
    }
}