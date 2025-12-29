using Blade.Combat;
using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerRollingState : PlayerState
    {
        private CharacterMovement _movement;
        private Vector3 _rollingDirection;
        private SkillComponent _skillCompo;
        private MovementDataSO _rollingMovement;
        
        public PlayerRollingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
            _skillCompo = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            _rollingMovement = _skillCompo.GetSkill<RollingSkill>().MovementData;
            _movement.CanManualMovement = false;
            _rollingDirection = _player.transform.forward;
            
            _movement.ApplyMovementData(_rollingDirection, _rollingMovement);
        }

        public override void Exit()
        {
            _movement.StopImmediately();
            _movement.CanManualMovement = true;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            if (_isTriggerCall)
            {
                _player.ChangeState("IDLE");
            }
        }
    }
}