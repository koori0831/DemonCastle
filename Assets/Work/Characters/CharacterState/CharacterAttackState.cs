using UnityEngine;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;
using Work.Utils.EventBus;
using Work.Utils.EventBus.Events;

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
            _movementCompo.SetCanMove(false);
            _movementCompo.SetMultiplier(1);
        }
    }
}
