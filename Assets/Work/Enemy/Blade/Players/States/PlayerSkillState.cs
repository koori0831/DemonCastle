using Blade.Entities;
using Blade.SkillSystem;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerSkillState : PlayerState
    {
        private SkillComponent _skillCompo;
        private CharacterMovement _movement;
        
        public PlayerSkillState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _skillCompo = entity.GetCompo<SkillComponent>();
            _movement = entity.GetCompo<CharacterMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.StopImmediately();
            Debug.Assert(_skillCompo != null && _skillCompo.CurrentSkill != null, 
                $"CurrentSkill is null, check skill component {_entity}");
            _skillCompo.CurrentSkill.UseSkill();
        }

        public override void Update()
        {
            base.Update();
            if(_isTriggerCall)
                _player.ChangeState("IDLE");
        }
    }
}