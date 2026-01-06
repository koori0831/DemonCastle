using System;
using Blade.Core;
using Blade.Events;
using TMPro;
using UnityEngine;

namespace Blade.UI
{
    public class ItemInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI goldText;
        [SerializeField] private GameEventChannelSO playerChannel;

        private void Awake()
        {
            playerChannel.AddListener<GoldChangeEvent>(HandleGoldChange);
        }

        private void OnDestroy()
        {
            playerChannel.RemoveListener<GoldChangeEvent>(HandleGoldChange);
        }

        private void HandleGoldChange(GoldChangeEvent evt)
        {
            goldText.SetText( evt.goldAmount.ToString());
        }
    }
}