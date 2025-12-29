using UnityEngine;
using Work.Utils.Datas;

namespace Work.Combat
{
    [CreateAssetMenu(fileName = "SkillData", menuName = "SO/Combat/SkillData", order = -10)]
    public class SkillData : DataSO
    {
        [field: SerializeField] public float cooldown { get; private set; } = 2f;
        [field: SerializeField] public bool isCanDamageImmunity { get; private set; } = false; // 피면
        [field: SerializeField] public bool isCanStiffImmunity { get; private set; } = false; // 경면
        [field: SerializeField] public string SkillClassPath { get; private set; }
        [field: SerializeField] public string SkillName { get; private set; } = "Attack Name";
    }
}
