using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

namespace Work.Characters.CharacterState
{
    public abstract class CharacterCanMoveState : State
    {
        protected Character _character;
        protected StateCompo _stateCompo;
        protected CharacterMovementCompo _movementCompo;

        protected CharacterCanMoveState(Entity entity, int animHash) : base(entity, animHash)
        {
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _movementCompo = _character.GetCompo<CharacterMovementCompo>();
        }


        public override void Enter()
        {
            base.Enter();
            _movementCompo.SetCanMove(true);
            Bus<PlayerMoveEvent>.Events += HandleMoveDirectionChanged;
        }

        public override void Exit()
        {
            base.Exit();
            Bus<PlayerMoveEvent>.Events -= HandleMoveDirectionChanged;
            _movementCompo.SetCanMove(false);
        }

        protected void HandleMoveDirectionChanged(PlayerMoveEvent evt)
        {
            _movementCompo.SetDirection(evt.MoveDirection);
            MoveHandler(evt);
        }

        protected virtual void MoveHandler(PlayerMoveEvent evt)
        {

        }
    }
}
