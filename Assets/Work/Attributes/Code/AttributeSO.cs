using UnityEngine;

namespace Work.Attributes.Code
{
    [CreateAssetMenu(fileName = "AttributeData", menuName = "SO/Attribute/AttributeData")]
    public class AttributeSO : ScriptableObject
    {
        [field: SerializeField] public AttributeEnums MyAttribute { get; private set; }
        [field: SerializeField] public AttributeEnums StrengthAttribute { get; private set; }
        [field: SerializeField] public AttributeEnums WeaknessAttribute { get; private set; }
    }
}