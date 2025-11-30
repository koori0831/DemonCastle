using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

namespace Work.Characters.CharacterState
{
    public class CharacterMoveState : State
    {
        private Character _character;
        private StateCompo _stateCompo;
        private CharacterMovementCompo _movementCompo;

        public CharacterMoveState(Entity entity, EntityAnimatorCompo animator, int animHash) : base(entity, animator, animHash)
        {
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _movementCompo = _character.GetCompo<CharacterMovementCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Move 진입");
            _movementCompo.SetCanMove(true);
            Bus<PlayerMoveEvent>.Events += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            Bus<PlayerMoveEvent>.Events -= HandleMoveDirectionChanged;
        }

        private void HandleMoveDirectionChanged(PlayerMoveEvent evt)
        {
            _movementCompo.SetDirection(evt.MoveDirection);

            if (evt.MoveDirection == Vector3.zero)
                _stateCompo.ChangeState("IDLE");
        }
    }
}