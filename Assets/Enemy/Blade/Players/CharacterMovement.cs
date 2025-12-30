using System;
using Blade.Combat;
using Blade.Entities;
using Blade.StatSystem;
using DG.Tweening;
using UnityEngine;

namespace Blade.Players
{
    public class CharacterMovement : MonoBehaviour, IEntityComponent, IAfterInitialize, IKnockBackable
    {
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private float gravity = -9.8f;
        [SerializeField] private CharacterController characterController;

        public bool CanManualMovement { get; set; } = true; //키보드 입력처리로 이동가능한가?
        private Vector3 _autoMovement;
        private float _moveSpeed = 8f;
        private float _autoMoveStartTime;
        private MovementDataSO _movementData;
        
        public bool IsGround => characterController.isGrounded;
        private Vector3 _velocity;
        public Vector3 Velocity => _velocity;
        
        private float _verticalVelocity;
        private Vector3 _movementDirection;

        private Entity _entity;
        private EntityStatCompo _statCompo;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
        }
        
        public void AfterInitialize()
        {
            _moveSpeed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 1f); //최초 한번은 초기화를 해야한다.
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentvalue, float previousvalue)
        {
            _moveSpeed = currentvalue;
        }

        public void SetMovementDirection(Vector2 movementInput)
        {
            _movementDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;
        }

        private void FixedUpdate()
        {
            CalculateMovement();
            ApplyGravity();
            Move();
        }

        private void CalculateMovement()
        {
            if (CanManualMovement)
            {
                _velocity = Quaternion.Euler(0, -45f, 0) * _movementDirection;
                _velocity *= _moveSpeed * Time.fixedDeltaTime;
            }
            else
            {
                float normalizeTime = (Time.time - _autoMoveStartTime) / _movementData.duration;
                float currentSpeed = _movementData.maxSpeed *
                                     _movementData.moveCurve.Evaluate(normalizeTime);
                Vector3 currentMovement = _autoMovement * currentSpeed;
                _velocity = currentMovement * Time.fixedDeltaTime;
                // _velocity = _autoMovement * Time.fixedDeltaTime;
            }

            if (_velocity.magnitude > 0 && CanManualMovement)
            {
                float rotationSpeed = 8f;
                Quaternion targetRotation = Quaternion.LookRotation(_velocity);
                _entity.transform.rotation = Quaternion.Lerp(_entity.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime); ;
            }
        }

        private void ApplyGravity()
        {
            if (IsGround && _verticalVelocity < 0)
                _verticalVelocity = -0.03f;
            else
                _verticalVelocity += gravity * Time.fixedDeltaTime;
            
            _velocity.y = _verticalVelocity;
        }

        private void Move()
        {
            characterController.Move(_velocity);
        }
        
        // public void SetAutoMovement(Vector3 autoMovement) => _autoMovement = autoMovement;
        
        public void StopImmediately()
        {
            _movementDirection = Vector3.zero;
        }

        public void KnockBack(Vector3 direction, MovementDataSO kbMovement)
        {
            _autoMovement = direction;
            _movementData = kbMovement;
            _autoMoveStartTime = Time.time;
        }

        public void ApplyMovementData(Vector3 playerDirection, MovementDataSO movementData)
        {
            _autoMovement = playerDirection;
            _autoMoveStartTime = Time.time;
            _movementData = movementData;
        }
    }
}