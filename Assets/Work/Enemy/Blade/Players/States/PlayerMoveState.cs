using Blade.Entities;
using Blade.FSM;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerMoveState : PlayerCanAttackState
    {
        private CharacterMovement _movement;
        private EntityVFX _entityVFX;
        
        private readonly string _footstepEffectname = "FootStep";
        public PlayerMoveState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _movement = entity.GetCompo<CharacterMovement>();
            _entityVFX = entity.GetCompo<EntityVFX>();
        }

        public override void Enter()
        {
            base.Enter();
            _entityVFX.PlayVFX(_footstepEffectname, Vector3.zero, Quaternion.identity);
        }

        public override void Exit()
        {
            _entityVFX.StopVFX(_footstepEffectname);
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            Vector2 movementKey = _player.PlayerInputSo.MovementKey;
            _movement.SetMovementDirection(movementKey);
            
            if(movementKey.magnitude < _inputThreshold)
                _player.ChangeState("IDLE");
        }
    }
}