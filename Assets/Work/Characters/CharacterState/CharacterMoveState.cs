using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;

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
            _character.CharacterDataContainer.OnMoveDirectionChanged += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            _character.CharacterDataContainer.OnMoveDirectionChanged -= HandleMoveDirectionChanged;
        }

        private void HandleMoveDirectionChanged()
        {
            _movementCompo.SetDirection(_character.CharacterDataContainer.MoveDirection);

            if (_character.CharacterDataContainer.MoveDirection == Vector3.zero)
                _stateCompo.ChangeState("IDLE");
        }
    }
}