using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Entities;

namespace Work.Characters.CharacterState
{
    public class CharacterDashState : CharacterCanMoveState
    {
        private CharacterMovementCompo _mover;

        private const float DASH_DURATION = 0.75f;
        private float _dashTimer = 0f;

        public CharacterDashState(Entity entity, int animHash) : base(entity, animHash)
        {
            _mover = entity.GetCompo<CharacterMovementCompo>();
        }

        public override void Enter()
        {
            _movementCompo.SetCanMove(false);
            _stateCompo.SetCanStateChange(false);
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
            _stateCompo.SetCanStateChange(true);
            _movementCompo.SetCanMove(true);
            _mover.SetDash(false);
        }

        public override void Update()
        {
            base.Update();
            _dashTimer += UnityEngine.Time.deltaTime;
            if (_dashTimer > DASH_DURATION)
            {
                _stateCompo.SetCanStateChange(true);
                _movementCompo.SetCanMove(true);
            }

            if(IsAnimationEndTriggered)
            {
                _stateCompo.ChangeState("IDLE", true);
            }
        }
    }
}
