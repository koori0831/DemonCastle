using UnityEngine;

namespace Work.Attributes.Code.Test
{
    public class AttributeTester : MonoBehaviour
    {
        [SerializeField] private AttributeSO data;
        [SerializeField] private AttributeEnums targetAttribute;
        private AttributeCalculate _calculate;

        private void Awake()
        {
            _calculate = new AttributeCalculate(data);
        }

        [ContextMenu("Test")]
        public void TestCalculate()
        {
            float damage = _calculate.Calculate(targetAttribute,5);
            Debug.Log("Damage : " + damage);
        }
    }
}