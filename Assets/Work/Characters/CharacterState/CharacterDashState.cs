using Blade.SkillSystem;
using UnityEngine;
using Work.Characters.Code;
using Work.Characters.Events;
using Work.Entities;

namespace Work.Characters.CharacterState
{
    public class CharacterDashState : CharacterCanMoveState
    {
        private CharacterMovementCompo _mover;
        private CharacterSkillCompo _skill;
        private const float DASH_DURATION = 0.75f;
        private float _dashTimer = 0f;

        public CharacterDashState(Entity entity, int animHash) : base(entity, animHash)
        {
            _mover = entity.GetCompo<CharacterMovementCompo>();
            _skill = entity.GetCompo<CharacterSkillCompo>();
        }


        public override void Enter()
        {
            base.Enter();
            _stateCompo.SetCanStateChange(false);
            _skill.UseSkill("Dash");
            _movementCompo.SetCanMove(false);
        }

        public override void Exit()
        {
            base.Exit();
            _stateCompo.SetCanStateChange(true);
            _movementCompo.SetCanMove(true);
            _mover.SetDash(false);
            _dashTimer = 0;
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

            if (IsAnimationEndTriggered)
            {
                _stateCompo.ChangeState("MOVE");
            }
        }

        protected override void MoveHandler(CharacterMoveEvent evt)
        {
            if (evt.MoveDirection != Vector3.zero)
            {
                _stateCompo.ChangeState("MOVE");
            }
        }
    }
}
