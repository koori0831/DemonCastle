using Blade.Entities;
using Blade.SkillSystem;

namespace Blade.Players.States
{
    public abstract class PlayerCanAttackState : PlayerState
    {
        protected SkillComponent _skillComponent;
        
        protected PlayerCanAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillComponent = entity.GetCompo<SkillComponent>();
        }

        public override void Enter()
        {
            base.Enter();
            _player.PlayerInputSo.OnAttackPressed += HandleAttackPressed;
            _player.PlayerInputSo.OnSkillPressed += HandleSkillKeyPressed;
        }

        public override void Exit()
        {
            _player.PlayerInputSo.OnAttackPressed -= HandleAttackPressed;
            _player.PlayerInputSo.OnSkillPressed -= HandleSkillKeyPressed;
            base.Exit();
        }

        private void HandleSkillKeyPressed(bool isPressed)
        {
            Skill currentSkill = _skillComponent.CurrentSkill;
            if (currentSkill == null) return;
            if (currentSkill.IsCooldown) return;

            if (isPressed && currentSkill is IChargeable { IsCharging: false } chargeable)
            {
                chargeable.StartCharge();
                _player.ChangeState("CHARGING");
            }else if (isPressed)
            {
                _player.ChangeState("SKILL");
            }
        }

        private void HandleAttackPressed()
        {
            _player.ChangeState("ATTACK");
        }
    }
}