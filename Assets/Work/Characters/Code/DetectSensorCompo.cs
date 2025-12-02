using UnityEngine;
using Work.Combat;
using Work.Entities;
using Work.Entities.Code;

namespace Work.Characters.Code
{
    public class DetectSensorCompo : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private DecalProjector decal;

        public Entity Owner { get; private set; }
        public IDamageable CurrentTarget { get; private set; } //나중에 다른걸로 바뀔 가능성 있음 , 타겟이 존재하는지와 같은 검사는 모두 얘를 통해서 이루어짐

        private SphereCollider _area;
        private float _detectionRadius = 5f;

        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _detectionRadius = Owner.EntityDataSO.AttackRange; //여기서 나중에는 실질적인 수치로 바꿔줘야함

            _area = GetComponent<SphereCollider>();
            _area.radius = _detectionRadius;

            decal.SetRadiuse(_detectionRadius);
            decal.SetActiveDecal(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            
        }
    }
}
