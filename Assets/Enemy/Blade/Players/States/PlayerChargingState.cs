using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerChargingState : PlayerState
    {
        private readonly int _chargingEndTrigger = Animator.StringToHash("CHARGING_END");
        private CharacterMovement _movement;
        private SkillComponent _skillComponent;
        private IChargeable _targetSkill;
        private bool _isReleased;
        
        public PlayerChargingState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
            _skillComponent = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
            _targetSkill = _skillComponent.CurrentSkill as IChargeable;
            Debug.Assert(_targetSkill != null, "Target skill is nul but you are in charging state");

            _player.PlayerInputSo.OnSkillPressed += HandleSkillReleased;
            _isReleased = false;
        }

        public override void Exit()
        {
            _player.PlayerInputSo.OnSkillPressed -= HandleSkillReleased;
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

            Vector3 mousePosition = _player.PlayerInputSo.GetWorldPosition();
            _player.RotateToTarget(mousePosition);
            
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        private void HandleSkillReleased(bool isPressed)
        {
            if (isPressed || _isReleased) return; //눌렸거나, 이미 떼진 상태

            _isReleased = true;
            _targetSkill.ReleaseCharge();
            _animator.SetParam(_chargingEndTrigger);
        }
    }
}