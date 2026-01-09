using System;

namespace Work.Utils.Helpers
{
    /// <summary>
    /// Helper들의 실행 순서를 결정하는 Attribut , 기본값 = 0
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HelperOrderAttribute : Attribute
    {
        public int Order { get; private set; }

        public HelperOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
