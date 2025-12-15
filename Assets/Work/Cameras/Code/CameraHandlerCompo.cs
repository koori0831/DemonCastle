using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using Work.Entities;

namespace Work.Cameras.Code
{

    public class CameraHandlerCompo : MonoBehaviour, IEntityComponent
    {
        private Entity _owner;

        [SerializeField] private GameObject impulseSourceObject;
        [SerializeField] private List<CameraShakingDataSO> defins;
        private Dictionary<string, ImpulsSourceObject> definsDict;

        public Entity Owner { get; set; }

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            definsDict = new Dictionary<string, ImpulsSourceObject>();
            foreach (var item in defins)
            {
                ImpulsSourceObject sourceObject = Instantiate(impulseSourceObject, transform).GetComponent<ImpulsSourceObject>();
                sourceObject.Init(item);
                sourceObject.name = item.name;
                definsDict.Add(item.shakingName, sourceObject);
            }

        }

        public void GenerateImpulse(string shakingName)
        {
            if (definsDict.TryGetValue(shakingName, out var sourceObject))
            {
                sourceObject.GenerateImpulse();
            }
            else
            {
                Debug.LogError($"Not Find Shaking Name : {shakingName}");
            }
        }



    }
}