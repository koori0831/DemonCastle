using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Blade.Entities
{
    public abstract class Entity : MonoBehaviour
    {
        public UnityEvent OnHitEvent;
        public UnityEvent OnDieEvent;
        
        public bool IsDead { get; set; }
        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            AddComponents();
            InitializeComponents();
            AfterInitializeComponents();
        }

        protected virtual void AfterInitializeComponents()
        {
            _components.Values.OfType<IAfterInitialize>()
                .ToList().ForEach(component => component.AfterInitialize());
        }

        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IEntityComponent>(true).ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }
        
        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }

        public T GetCompo<T>() where T : IEntityComponent
            => (T)_components.GetValueOrDefault(typeof(T));
        
        public IEntityComponent GetCompo(Type type) 
            => _components.GetValueOrDefault(type);

        public void EntityDestroy()
        {
            Destroy(gameObject);
        }
        
        public void RotateToTarget(Vector3 targetPosition, bool isSmooth = false)
        {
            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);

            if (isSmooth)
            {
                const float smoothSpeed = 15f;
                //Lerp 차이점.
                transform.rotation = Quaternion.RotateTowards(transform.rotation, 
                    targetRotation, Time.deltaTime * smoothSpeed);
            }
            else
            {
                transform.rotation = targetRotation;
            }
        }
    }
}