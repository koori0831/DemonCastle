using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

namespace Work.Characters.CharacterState
{
    public class CharacterAttackState : State
    {
        private Character _character;
        private CharacterAttackCompo _attackCompo;
        private CharacterAnimatorCompo _animatorCompo;
        private CharacterMovementCompo _movementCompo;
        private StateCompo _stateCompo;

        private int _attackIndex;

        public CharacterAttackState(Entity entity, int animHash) : base(entity, animHash)
        {
            _character = entity as Character;

            _attackCompo = _character.GetCompo<CharacterAttackCompo>(true);
            _animatorCompo = _character.GetCompo<CharacterAnimatorCompo>(true);
            _movementCompo = _character.GetCompo<CharacterMovementCompo>(true);
            _stateCompo = _character.GetCompo<StateCompo>(true);
        }

        public override void Enter()
        {
            _animatorCompo.SetParam(Animator.StringToHash("ATTACK_COUNT"),(float)_attackIndex);
            base.Enter();
            _attackIndex = _attackCompo.CurrentAttackCount;
            Bus<PlayerMoveEvent>.Events += HandleMoveDirectionChanged;
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
            Bus<PlayerMoveEvent>.Events -= HandleMoveDirectionChanged;
        }

        private void HandleMoveDirectionChanged(PlayerMoveEvent evt)
        {
            _movementCompo.SetDirection(evt.MoveDirection);
        }
    }
}
