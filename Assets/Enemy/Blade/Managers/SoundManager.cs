using System;
using System.Collections.Generic;
using Blade.Core;
using Blade.Events;
using Blade.SoundSystem;
using GondrLib.Dependencies;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Audio;

namespace Blade.Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO soundEvtChannel;
        [SerializeField] private PoolItemSO soundPlayerItem;

        [Inject] private PoolManagerMono _poolManager;
        
        private Dictionary<int, SoundPlayer> _playerDictionary = new Dictionary<int, SoundPlayer>();

        private void Awake()
        {
            soundEvtChannel.AddListener<PlaySFXEvent>(HandlePlayerSFX);
            soundEvtChannel.AddListener<StopSoundEvent>(HandleStopSound);
        }

        private void OnDestroy()
        {
            soundEvtChannel.RemoveListener<PlaySFXEvent>(HandlePlayerSFX);
            soundEvtChannel.RemoveListener<StopSoundEvent>(HandleStopSound);
        }
        
        private void HandlePlayerSFX(PlaySFXEvent evt)
        {
            SoundPlayer player = _poolManager.Pop<SoundPlayer>(soundPlayerItem);
            player.transform.position = evt.position;
            player.PlaySound(evt.soundClip);
            if (evt.channel > 0 && evt.soundClip.isLoop)
            {
                if (_playerDictionary.TryGetValue(evt.channel, out SoundPlayer beforePlayer))
                {
                    beforePlayer.StopAndGoToPool();
                    _playerDictionary.Remove(evt.channel);
                }
                _playerDictionary.Add(evt.channel, player);
            }
            else if(evt.channel <= 0 && evt.soundClip.isLoop)
            {
                Debug.LogWarning($"사운드가 루프 설정이 되었으나 채널이 0 이하입니다. {evt.soundClip.name}");
            }
        }
        
        private void HandleStopSound(StopSoundEvent evt)
        {
            if (_playerDictionary.TryGetValue(evt.channel, out SoundPlayer beforePlayer))
            {
                beforePlayer.StopAndGoToPool();
                _playerDictionary.Remove(evt.channel);
            }
        }
        
    }
}