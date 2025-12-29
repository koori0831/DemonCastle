using System;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace Blade.SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private AudioMixerGroup bgmGroup;
        
        private AudioSource _audioSource;
        private Pool _myPool;
        
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        public GameObject GameObject => gameObject;
        
        public void SetUpPool(Pool pool) => _myPool = pool;

        public void ResetItem()
        {
            //do nothing   
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(SoundSO clipData)
        {
            _audioSource.outputAudioMixerGroup = clipData.audioType switch
            {
                SoundSO.AudioTypes.SFX => sfxGroup,
                SoundSO.AudioTypes.MUSIC => bgmGroup,
                _ => sfxGroup
            };
            
            _audioSource.volume = clipData.volume;
            _audioSource.pitch = clipData.pitch;

            if (clipData.isRandomizePitch)
            {
                _audioSource.pitch += Random.Range(-clipData.randomPitchModifier, clipData.randomPitchModifier);
            }
            
            _audioSource.clip = clipData.clip;
            _audioSource.loop = clipData.isLoop;

            if (!clipData.isLoop)
            {
                float time = _audioSource.clip.length + 0.2f;
                DisableSoundTimer(time);
            }
            _audioSource.Play();
        }

        private async void DisableSoundTimer(float time)
        {
            await Awaitable.WaitForSecondsAsync(time);
            _myPool.Push(this);
        }

        public void StopAndGoToPool()
        {
            _audioSource.Stop();
            _myPool.Push(this);
        }
    }
}