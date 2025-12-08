using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.Characters.Stats.Code;
using Work.Entities.Code;

namespace Work.Entities
{
    public class Entity : MonoBehaviour, IDamageable
    {
        #region Members 

        private Dictionary<Type, IEntityComponent> components = new();
        public delegate void OnTakeDamaged(Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0);
        public event OnTakeDamaged OnTakeDamageEvent;

        public StatContainer StatContainer { get; private set; }
        public AbstractEntityDataSO EntityDataSO { get; private set; }
        public Transform Transform => this != null ? transform : null;

        public Action<IDamageable> OnDeadEvent { get; set; }

        #endregion

        #region Init
        public void Init(AbstractEntityDataSO entityData)
        {
            EntityDataSO = entityData;
            StatContainer = new StatContainer();
            StatContainer.InitailizeStatContainer(EntityDataSO);

            GetEntityComponents();
            InitializeCompo();
            AfterInitCompo();
        }

        private void AfterInitCompo()
        {
            components.ToList().ForEach(kvp =>
            {
                if (kvp.Value is IAfterInitCompo afterInitCompo)
                    afterInitCompo.AfterInit();
            });
        }

        private void InitializeCompo() => components.Values.ToList().ForEach(compo => compo.InitCompo(this));

        public void GetEntityComponents() => components = GetComponentsInChildren<IEntityComponent>(true).ToDictionary(compo => compo.GetType());
        #endregion

        #region Methods
        public T GetCompo<T>(bool isAssignable = false) where T : class, IEntityComponent
        {
            if (components.TryGetValue(typeof(T), out var compo))
                return compo as T;
            if (isAssignable == false)
            {
                Debug.LogError($"Not Find {typeof(T)}");
                return null;
            }

            foreach (var kvp in components)
            {
                if (kvp.Value is T tComp)
                    return tComp;
            }

            Debug.LogError($"Not Find {typeof(T)}");
            return null;
        }

        public void TakeDamage(Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0)
        {
            OnTakeDamageEvent?.Invoke(attacker,damage,normal,isKnockback,knockbackPower);
        }
        #endregion
    }
}