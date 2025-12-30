using UnityEngine;

namespace Blade.UI.TabMenu
{
    [CreateAssetMenu(fileName = "tab data", menuName = "SO/UI/Tab data", order = 0)]
    public class TabDataSO : ScriptableObject
    {
        [field: SerializeField] public string TabName { get; private set; } = "Tab";
        [field: SerializeField] public string TabButtonText { get; private set; } = "Tab";

        [SerializeField] private string description;
    }
}