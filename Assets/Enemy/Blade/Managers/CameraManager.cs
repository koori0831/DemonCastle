using System;
using Blade.Core;
using Blade.Events;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Managers
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameEventChannelSO cameraChannel;

        private CinemachineImpulseSource _impulseSource;

        private void Awake()
        {
            cameraChannel.AddListener<ImpulseEvent>(HandleCameraImpulse);
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void OnDestroy()
        {
            cameraChannel.RemoveListener<ImpulseEvent>(HandleCameraImpulse);
        }

        private void HandleCameraImpulse(ImpulseEvent evt)
        {
            _impulseSource.GenerateImpulse(evt.power);
        }
    }
}