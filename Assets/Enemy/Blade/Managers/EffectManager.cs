using System;
using System.Collections.Generic;
using System.Linq;
using Blade.Core;
using Blade.Effects;
using Blade.Events;
using Blade.UI;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace Blade.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private GameEventChannelSO effectEventChannel;

        [SerializeField] private PoolItemSO popupItem;
        [SerializeField] private TextInfoSO[] textInfos;
        private Dictionary<int, TextInfoSO> _textInfoDictionary;
        
        private void Awake()
        {
            effectEventChannel.AddListener<PlayPoolEffect>(HandlePlayPoolEffect);
            effectEventChannel.AddListener<PopupTextEvent>(HandlePopupText);

            _textInfoDictionary = textInfos.ToDictionary(info => info.nameHash);
        }

        private void OnDestroy()
        {
            effectEventChannel.RemoveListener<PlayPoolEffect>(HandlePlayPoolEffect);
            effectEventChannel.RemoveListener<PopupTextEvent>(HandlePopupText);
        }

        private void HandlePopupText(PopupTextEvent evt)
        {
            PopupText popupText = _poolManager.Pop<PopupText>(popupItem);
            TextInfoSO textInfo = _textInfoDictionary.GetValueOrDefault(evt.textTypeHash);
            Debug.Assert(textInfo != null, $"Request text info is null {evt.textTypeHash}");
            
            popupText.ShowPopupText(evt.textMsg, textInfo, evt.position, evt.showDuration);
        }

        private async void HandlePlayPoolEffect(PlayPoolEffect evt)
        {
            PoolingEffect effect = _poolManager.Pop<PoolingEffect>(evt.effectItem);
            effect.PlayVFX(evt.position, evt.rotation);

            await Awaitable.WaitForSecondsAsync(evt.duration);
            _poolManager.Push(effect);
        }
    }
}