using System;
using Blade.Enemies.Skeleton;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Random = UnityEngine.Random;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "MoveAround", story: "[Self] move around [Target]", category: "Action", id: "cea7f2e1bf91e9196ad2d27f629add0f")]
    public partial class MoveAroundAction : Action
    {
        [SerializeReference] public BlackboardVariable<CommonEnemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;
        [SerializeReference] public BlackboardVariable<float> AroundDistance = new BlackboardVariable<float>(4f);
        [SerializeReference] public BlackboardVariable<float> DeltaDistance = new BlackboardVariable<float>(1f);
        
        private Transform _selfTrm;
        private Transform _targetTrm;
        private NavMovement _movement;
        private float _aroundDirection; //동전던지기로 방향을 정한다고 생각하면 된다.
        

        protected override Status OnStart()
        {
            _selfTrm = Self.Value?.transform;
            _targetTrm = Target.Value;
            if(_selfTrm == null || _targetTrm == null)
            {
                Debug.LogError("Self or Target is null in MoveAroundAction.");
                return Status.Failure;
            }
            _movement = Self.Value.GetCompo<NavMovement>();
            if (_movement == null)
                return Status.Failure;
            
            _aroundDirection = Random.value < 0.5f ? 1f : -1f; //랜덤으로 시계방향 또는 반시계방향을 정한다.
            SetNextDestination();
            return Status.Running;
        }

        private void SetNextDestination()
        {
            float angle = Random.Range(25f, 35f) * _aroundDirection; //25도에서 35도 사이의 각도를 랜덤으로 정한다.

            Vector3 direction = _selfTrm.position - _targetTrm.position;
            direction.y = 0;
            direction = Quaternion.Euler(0, angle, 0) * direction; //정해진 각도만큼 회전시킨다.
            
            Vector3 destination = _targetTrm.position + direction.normalized * AroundDistance;
            _movement.SetDestination(destination);
        }

        protected override Status OnUpdate()
        {
            if(_movement.RemainDistance < 0.5f) 
                SetNextDestination(); //다음위치로 계속 이동

            float distance = Vector3.Distance(_selfTrm.position, _targetTrm.position);

            if (distance > AroundDistance + DeltaDistance) //플레이어가 도망간거임.
            {
                return Status.Failure;
            }
            
            return Status.Running; //계속 돌린다.
        }
    }
}

