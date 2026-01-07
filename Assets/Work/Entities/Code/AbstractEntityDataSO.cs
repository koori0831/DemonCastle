using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Stats.Code;

namespace Work.Entities.Code
{
    [Serializable]
    public class AnimationData
    {
        public RuntimeAnimatorController AnimatorController;
        public GameObject visualPrefab;
    }

    public abstract class AbstractEntityDataSO : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        
    }
}
