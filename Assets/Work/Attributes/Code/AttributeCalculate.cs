namespace Work.Attributes.Code
{
    public class AttributeCalculate
    {
        private AttributeSO _attributeData;

        public AttributeCalculate(AttributeSO attributeSO)
        {
            _attributeData = attributeSO;
        }

        public float Calculate(AttributeEnums targetAttribute, float damage)
        {
            if ((targetAttribute & _attributeData.WeaknessAttribute) != 0)
            {
                damage *= 0.5f;
            }
            else if ((targetAttribute & _attributeData.StrengthAttribute) != 0)
            {
                damage *= 1.5f;
            }

            return damage;
        }

        public AttributeEnums GetMyAttribute()
        {
            return _attributeData.MyAttribute;
        }
    }
}
