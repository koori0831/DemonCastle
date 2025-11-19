using UnityEngine;
using Work.Entities;

namespace Work.Characters.FSM.Code
{
    public abstract class State
    {
        protected Entity _owner;
        protected EntityAnimatorCompo _animator;
        protected int _animHash;
        public bool IsAnimationEndTriggered { get; private set; }

        public State(Entity entity, EntityAnimatorCompo animator, int animHash)
        {
            _owner = entity;
            _animator = animator;
            _animHash = animHash;
        }

        public virtual void Enter()
        {
            _animator.SetParam(_animHash,true);
        }

        public virtual void Exit()
        {
            _animator.SetParam(_animHash,false);
            IsAnimationEndTriggered = false;
        }

        public virtual void Update()
        {

        }
    }
}