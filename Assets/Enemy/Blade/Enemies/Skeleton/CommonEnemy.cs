using System;
using Blade.Combat;
using Blade.Core;
using Blade.Enemies.BT.Events;
using Blade.Entities;
using Blade.Events;
using UnityEngine;

namespace Blade.Enemies.Skeleton
{
    public class CommonEnemy : Enemy, IKnockBackable
    {
        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }
        
        //의존성 제거는 나중에 할께.
        private StateChangeEvent _stateChangeChannel;
        private NavMovement _movement;
        private RagDollCompo _ragDollCompo;
        private EntityActionData _actionData; //이부분은 나중에 정리하자.

        [field: SerializeField] public bool IsBattleMode { get; set; } = false;
        
        protected override void Awake()
        {
            base.Awake();
            _movement = GetCompo<NavMovement>();
            _ragDollCompo = GetCompo<RagDollCompo>();
            _actionData = GetCompo<EntityActionData>();
            
            PlayerChannel.AddListener<PlayerDeadEvent>(HandlePlayerDead);
        }

        protected override void Start()
        {
            base.Start();
            _stateChangeChannel = GetBlackboardVariable<StateChangeEvent>("StateChannel").Value;
            OnDieEvent.AddListener(HandleDeadEvent);
        }
        
        private void OnDestroy()
        {
            OnDieEvent.RemoveListener(HandleDeadEvent);
            PlayerChannel.RemoveListener<PlayerDeadEvent>(HandlePlayerDead);
        }

        private void HandlePlayerDead(PlayerDeadEvent evt)
        {
            var target = GetBlackboardVariable<Transform>("Target");
            target.Value = null;
            _stateChangeChannel.SendEventMessage(EnemyState.IDLE);
        }

        private void HandleDeadEvent()
        {
            if (IsDead) return;
            IsDead = true;
            _stateChangeChannel.SendEventMessage(EnemyState.DEAD);
            
            // _ragDollCompo.SetColliderActive(true);
            // _ragDollCompo.SetRagDollActive(true);

            const float force = -30f;
            _ragDollCompo.AddForceToRagDoll(_actionData.HitNormal * force, _actionData.HitPoint);
        }
        
        public void KnockBack(Vector3 direction, MovementDataSO movementData)
        {
            _movement.KnockBack(direction, movementData);
        }

        public void HandleChildAnimatorMove(Vector3 deltaPosition, Quaternion deltaRotation)
        {
            transform.position += deltaPosition;
            transform.rotation = deltaRotation * transform.rotation;
        }

        public void SetBattleMode()
        {
            if(IsBattleMode || IsDead) return;
            IsBattleMode = true;

            var stateVar = GetBlackboardVariable<EnemyState>("CurrentState");
            if (stateVar != null && stateVar.Value != EnemyState.HIT)
            {
                _stateChangeChannel.SendEventMessage(EnemyState.CHASE);
            }
        }
    }
}