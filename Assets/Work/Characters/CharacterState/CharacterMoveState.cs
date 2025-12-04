using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

namespace Work.Characters.CharacterState
{
    public class CharacterMoveState : CharacterCanMoveState
    {

        public CharacterMoveState(Entity entity, int animHash) : base(entity, animHash)
        {
        }

        protected override void MoveHandler(PlayerMoveEvent evt)
        {
            if(evt.MoveDirection == Vector3.zero)
            {
                _stateCompo.ChangeState("IDLE");
            }
        }
    }
}