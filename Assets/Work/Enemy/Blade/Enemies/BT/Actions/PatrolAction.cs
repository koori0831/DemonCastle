using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "[Self] patrol with [waypoints]", category: "Action", id: "6a7977934a3773f7546c639d22ff054f")]
    public partial class PatrolAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;
        [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;

        private int _currentWayPoint;
        private NavMovement _navMovement;
        
        protected override Status OnStart()
        {
            Debug.Log("patrol start");
            Initialize();
            Vector3 nextDestination = Waypoints.Value[_currentWayPoint].transform.position;
            _navMovement.SetDestination(nextDestination);
            return Status.Running;
        }

        private void Initialize()
        {
            //초기에 네비게이션 컴포넌트가 없다면 가져와주기
            if(_navMovement == null)
                _navMovement = Self.Value.GetCompo<NavMovement>();
        }

        protected override Status OnUpdate()
        {
            if (_navMovement.IsArrived)
            {
                return Status.Success;
            }
            return Status.Running;
        }

        protected override void OnEnd()
        {
            _currentWayPoint = (_currentWayPoint + 1) % Waypoints.Value.Count;
        }
    }
}

