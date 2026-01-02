using System;
using UnityEngine;
using UnityEngine.Events;

namespace Blade.Entities
{
    public class EntityAnimator : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private Animator animator;

        public UnityEvent<Vector3, Quaternion> OnAnimatorMoveEvent;
        
        public bool ApplyRootMotion
        {
            get => animator.applyRootMotion;
            set => animator.applyRootMotion = value;
        }
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
        
        public void HandleDeadEvent()
        {
            animator.enabled = false;
        }

        public void SetParam(int hash, float value) => animator.SetFloat(hash, value);
        public void SetParam(int hash, int value) => animator.SetInteger(hash, value);
        public void SetParam(int hash, bool value) => animator.SetBool(hash, value);
        public void SetParam(int hash) => animator.SetTrigger(hash);

        public void SetParam(int hash, float value, float dampTime)
            => animator.SetFloat(hash, value, dampTime, Time.deltaTime);

        private void OnAnimatorMove()
        {
            OnAnimatorMoveEvent?.Invoke(animator.deltaPosition, animator.deltaRotation);
        }
    }
}