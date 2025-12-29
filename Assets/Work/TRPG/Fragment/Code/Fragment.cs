using UnityEngine;

namespace Work.TRPG.Fragment
{
    public abstract class Fragment : ScriptableObject
    {
        public string fragmentName;
        public string description;
        public Sprite icon;
    }
}