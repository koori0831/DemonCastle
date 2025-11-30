using System;
using UnityEngine;

namespace Work.Attributes.Code
{
    [Flags]
    public enum AttributeEnums
    {
        None = 0,
        Courage = 1 << 0,
        Hatred = 1 << 1,
        Greed = 1 << 2,
        Misery = 1 << 3,
        Misfortune = 1 << 4,
        Luck = 1 << 5,
    }
}