using System;
using System.Collections;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;

namespace Work.Characters.CharacterState
{
    public class CharacterIdleState : State
    {
        private Character _character;
        private StateCompo _stateCompo;
        private CharacterMovementCompo _movementCompo;

        public CharacterIdleState(Entity entity, EntityAnimatorCompo animator, int animHash) : base(entity, animator, animHash)
        {
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _movementCompo = _character.GetCompo<CharacterMovementCompo>();
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle 진입");
            _movementCompo.SetCanMove(false);
            _character.CharacterDataContainer.OnMoveDirectionChanged += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            _character.CharacterDataContainer.OnMoveDirectionChanged -= HandleMoveDirectionChanged;
        }

        private void HandleMoveDirectionChanged()
        {
            if (_character.CharacterDataContainer.MoveDirection != Vector3.zero)
            {
                _stateCompo.ChangeState("MOVE");
            }
        }
    }
}