using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.Cameras.Code
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class ImpulsSourceObject : MonoBehaviour
    {
        private CinemachineImpulseSource impulseSource;
        private float impulseForce = 1f;

        public void Init(CameraShakingDataSO data)
        {
            impulseSource = GetComponent<CinemachineImpulseSource>();
            impulseSource.ImpulseDefinition = data.GetDefinition();
            impulseSource.DefaultVelocity = data.defaultVelocity;
            impulseForce = data.impulsForce;

            impulseSource.ImpulseDefinition.OnValidate();
        }

        public void GenerateImpulse()
        {
            impulseSource.GenerateImpulse(impulseForce);
        }

    }
}
