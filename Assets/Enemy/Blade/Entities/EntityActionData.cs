using Blade.Combat;
using UnityEngine;

namespace Blade.Entities
{
    public class EntityActionData : MonoBehaviour, IEntityComponent
    {
        public Vector3 HitPoint { get; set; }
        public Vector3 HitNormal { get; set; }
        public bool HitByPowerAttack { get; set; }
        public DamageData LastDamageData { get; set; }
        
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }
    }
}