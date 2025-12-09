using UnityEngine;

namespace Work.TRPG.Code
{
    public abstract class Fragment : ScriptableObject
    {
        public string fragmentName;
        public string description;
        public Sprite icon;
    }
}