using System;
using Blade.Combat;
using Blade.Entities;
using Blade.StatSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace Blade.Enemies
{
    public class NavMovement : MonoBehaviour, IEntityComponent, IKnockBackable, IAfterInitialize
    {
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private float stopOffset = 0.05f;
        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private bool isUpdateRotation = false;
        [SerializeField] private LayerMask whatIsWall;
        [SerializeField] private StatSO moveSpeedStat;
        
        private Entity _entity;
        private EntityStatCompo _statCompo;
        private Transform _lookAtTrm;
        
        public bool IsArrived => !agent.pathPending && agent.remainingDistance < agent.stoppingDistance + stopOffset;
        public float RemainDistance => agent.pathPending ? -1 : agent.remainingDistance;
        public Vector3 Velocity => agent.velocity;
        
        public bool UpdateRotation
        {
            get => agent.updateRotation;
            set => agent.updateRotation = value;
        }
        
        private float _speedMultiplier = 1f;
        public float SpeedMultiplier
        {
            get => _speedMultiplier;
            set
            {
                _speedMultiplier = value;
                agent.speed = _statCompo.GetStat(moveSpeedStat).Value * _speedMultiplier;
            }
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStatCompo>();
            agent.updateRotation = isUpdateRotation;
        }
        
        public void AfterInitialize()
        {
            agent.speed = _statCompo.SubscribeStat(moveSpeedStat, HandleMoveSpeedChange, 1f);
        }

        private void HandleMoveSpeedChange(StatSO stat, float currentvalue, float previousvalue)
        {
            agent.speed = currentvalue * _speedMultiplier;
        }

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(moveSpeedStat, HandleMoveSpeedChange);
            _entity.transform.DOKill(); //사망시 넉백중이던 doTween을 중지합니다.
        }
        
        public async void KnockBack(Vector3 direction, MovementDataSO kbMovement)
        {
            //여기서 넉백 저항력이 있다면 반영해서 저항해줘야 한다.
            SetStop(true); //네비게이션은 정지시켜주고

            float duration = kbMovement.duration;
            float currentTime = 0;
            float maxSpeed = kbMovement.maxSpeed;
            AnimationCurve moveCurve = kbMovement.moveCurve;

            while (currentTime < duration)
            {
                float normalizeTime = currentTime / duration;
                float currentSpeed = maxSpeed * moveCurve.Evaluate(normalizeTime);
                Vector3 currentMovement = direction * currentSpeed;
                _entity.transform.Translate(currentMovement * Time.fixedDeltaTime, Space.World);
                currentTime += Time.fixedDeltaTime;
                await Awaitable.FixedUpdateAsync();
            }
            //여기서 추가 작업을 안해주면 넉백이 이상해진다. 일단 이상하게 해서 봅시다.
            WarpToPosition(_entity.transform.position);
            SetStop(false); //넉백이 끝나면 다시 네비게이션을 시작합니다.
            
        }

        private Vector3 GetKnockBackEndPosition(Vector3 force)
        {
            Vector3 startPosition = _entity.transform.position + new Vector3(0, 0.5f); //위로 올려서 위치 잡고
            if(Physics.Raycast(startPosition, force.normalized, 
                   out RaycastHit hit, force.magnitude, whatIsWall))
            {
                Vector3 hitPoint = hit.point - force.normalized * 0.5f;
                hitPoint.y = _entity.transform.position.y;
                return hitPoint;
            }
            //벽에 안부딛혔다면 넉백거리 그대로 적용
            return _entity.transform.position + force;
        }

        private void Update()
        {
            if (_lookAtTrm != null)
            {
                LookAtTarget(_lookAtTrm.position);
            }
            else if (agent.hasPath && agent.isStopped == false)
            {
                LookAtTarget(agent.steeringTarget);
            }
        }
        
        public Quaternion LookAtTarget(Vector3 target, bool isSmooth = true)
        {
            Vector3 direction = target - _entity.transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction.normalized);

            if (isSmooth)
            {
                _entity.transform.rotation = Quaternion.Slerp(_entity.transform.rotation, 
                                                lookRotation, Time.deltaTime * rotateSpeed);
            }
            else
            {
                _entity.transform.rotation = lookRotation; 
            }

            return lookRotation;
        }

        public void SetLookAtTarget(Transform target)
        {
            _lookAtTrm = target;
            UpdateRotation = _lookAtTrm == null; //쳐다볼 타겟이 있으면 UpdateRotation을 false로 설정
        }

        public void SetStop(bool isStop) => agent.isStopped = isStop;
        public void SetVelocity(Vector3 velocity) => agent.velocity = velocity;
        public void SetSpeed(float speed) => agent.speed = speed;
        public void SetDestination(Vector3 destination) => agent.SetDestination(destination);

        //네브 에이전트를 특정위치로 전송시키는 함수.(Transform을 옮기지 않는다)
        public void WarpToPosition(Vector3 position) => agent.Warp(position);
    }
}