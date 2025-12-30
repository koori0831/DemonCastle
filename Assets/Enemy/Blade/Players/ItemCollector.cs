using System;
using Blade.Entities;
using Blade.Items;
using UnityEngine;

namespace Blade.Players
{
    public class ItemCollector : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private float collectDistance = 2f;
        [SerializeField] private LayerMask collectableLayer;

        private Entity _entity;
        private Collider[] _overlapResults;
        private const int MaxOverlapCount = 7;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _overlapResults = new Collider[MaxOverlapCount];
        }

        private void FixedUpdate()
        {
            int count = Physics.OverlapSphereNonAlloc(transform.position, collectDistance,
                _overlapResults, collectableLayer);

            for (int i = 0; i < count; i++)
            {
                if (_overlapResults[i].TryGetComponent(out ICollectable collectable))
                {
                    if (collectable.CanCollect == false) continue;
                    collectable.Collect(_entity);
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, collectDistance);
        }
    }
}