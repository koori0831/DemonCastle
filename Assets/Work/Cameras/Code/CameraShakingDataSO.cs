using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Work.Cameras.Code
{
    [CreateAssetMenu(fileName = "CameraShakingData", menuName = "SO/Cameras/CameraShakingData", order = 0)]
    public class CameraShakingDataSO : ScriptableObject
    {
        [Header("이름")]
        public string shakingName;
        [Header("카메라 흔들림 방향")]
        public Vector3 defaultVelocity;
        [Header("카메라 흔들림 정도")]
        //[SerializeField] private List<CinemachineImpulseDefinition> defins;
        public float impulsForce = 1f;
        public CinemachineImpulseDefinition[] definition = new CinemachineImpulseDefinition[1];

        public CinemachineImpulseDefinition GetDefinition()
        {
            if (definition.Length == 0)
                return null;
            return definition[UnityEngine.Random.Range(0, definition.Length)];
        }

        
    }
}
