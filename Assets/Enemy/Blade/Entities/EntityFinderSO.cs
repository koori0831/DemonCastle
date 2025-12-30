using UnityEngine;

namespace Blade.Entities
{
    [CreateAssetMenu(fileName = "EntityFinder", menuName = "SO/EntityFinder", order = 0)]
    public class EntityFinderSO : ScriptableObject
    {
        public Work.Entities.Entity Target { get; private set; }

        public void SetTarget(Work.Entities.Entity target)
        {
            Target = target;
        }
    }
}