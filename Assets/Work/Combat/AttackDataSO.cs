using UnityEngine;

namespace Work.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = -10)]
    public class AttackDataSO : ScriptableObject
    {
        [field: SerializeField] public string AttackName { get; private set; } = "Attack Name";
        [field: SerializeField] public int AttackCount { get; private set; } = 0;
        [field: SerializeField] public float AttackDamage { get; private set; } = 5;
    }
}