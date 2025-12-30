using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Events;
using Work.Entities;

namespace Work.Characters.CharacterState
{
    public class CharacterAttackState : CharacterCanMoveState
    {
        private CharacterAttackCompo _attackCompo;

        public CharacterAttackState(Entity entity, int animHash) : base(entity, animHash)
        {
            _attackCompo = _character.GetCompo<CharacterAttackCompo>(true);
        }

        public override void Enter()
        {
            base.Enter();
            _attackCompo.isAttacking = true;
            _movementCompo.SetCanMove(true);
            _movementCompo.SetMultiplier(0.4f);
        }

        public override void Update()
        {
            base.Update();

            if (IsAnimationEndTriggered)
                _stateCompo.ChangeState("IDLE");

        }

        public override void Exit()
        {
            base.Exit();
            _attackCompo.isAttacking = false;
            _attackCompo.AddAttackCount();
            _movementCompo.SetCanMove(false);
            _movementCompo.SetMultiplier(1);
        }

        protected override void MoveHandler(CharacterMoveEvent evt)
        {
            if(evt.MoveDirection == Vector3.zero)
            {
                _animator.Anim.SetLayerWeight(1, 0f);
            }
            else
            {
                _animator.Anim.SetLayerWeight(1, 1f);
            }
            
        }
    }
}
