using Blade.Combat;
using Blade.Entities;
using UnityEngine;

namespace Blade.Players.States
{
    public class PlayerAttackState : PlayerState
    {
        private PlayerAttackCompo _attackCompo;
        private CharacterMovement _movement;
        
        public PlayerAttackState(Entity entity, int animationHash) : base(entity, animationHash)
        {
            _attackCompo = entity.GetCompo<PlayerAttackCompo>();
            _movement = entity.GetCompo<CharacterMovement>();
        }

        public override void Enter()
        {
            base.Enter();
            _attackCompo.Attack();

            ApplyAttackData();
            //Vector3 movement = playerDirection * currentAtkData.m;
            _movement.CanManualMovement = false;
            // _movement.SetAutoMovement(movement);
        }

        private void ApplyAttackData()
        {
            AttackDataSO currentAtkData = _attackCompo.GetCurrentAttackData();
            Vector3 playerDirection = GetPlayerDirection(); //이거 나중에 바뀜
            _player.transform.rotation = Quaternion.LookRotation(playerDirection); //이걸 위해서 변경
            _movement.ApplyMovementData(playerDirection, currentAtkData.movementData);
        }

        private Vector3 GetPlayerDirection()
        {
            if(_attackCompo.useMouseDirection == false)
                return _player.transform.forward;
            
            Vector3 targetPosition = _player.PlayerInputSo.GetWorldPosition();
            Vector3 direction = targetPosition - _player.transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        public override void Exit()
        {
            _attackCompo.EndAttack();
            _movement.CanManualMovement = true;
            // _movement.StopImmediately();
            base.Exit();
        }

        public override void Update()
        {
            base.Update();
            if(_isTriggerCall) 
                _player.ChangeState("IDLE");
        }
    }
}