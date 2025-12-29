using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerDeadState : PlayerState
    {
        private EntityActionData _actionData;
        private CharacterMovement _movement;
        
        public PlayerDeadState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _actionData = entity.GetCompo<EntityActionData>();
            _movement = entity.GetCompo<CharacterMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _movement.CanManualMovement = false;
            Vector3 lookDirection = _actionData.HitNormal;
            lookDirection.y = 0f; 
            _entity.transform.rotation = Quaternion.LookRotation(lookDirection.normalized);
        }
    }
}