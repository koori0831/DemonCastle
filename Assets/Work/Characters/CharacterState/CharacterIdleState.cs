using System;
using System.Collections;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

namespace Work.Characters.CharacterState
{
    public class CharacterIdleState : State
    {
        private Character _character;
        private StateCompo _stateCompo;
        private CharacterMovementCompo _movementCompo;

        public CharacterIdleState(Entity entity,int animHash) : base(entity, animHash)
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
            Bus<PlayerMoveEvent>.Events += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            Bus<PlayerMoveEvent>.Events -= HandleMoveDirectionChanged;
        }

        private void HandleMoveDirectionChanged(PlayerMoveEvent evt)
        {
            if (evt.MoveDirection != Vector3.zero)
            {
                _stateCompo.ChangeState("MOVE");
            }
        }
    }
}