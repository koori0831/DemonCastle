using UnityEngine;

namespace Blade.Items
{
    [CreateAssetMenu(fileName = "Drop table", menuName = "SO/Item/Drop table", order = 0)]
    public class DropTableSO : ScriptableObject
    {
        public int dropExp;
        public int minDropGold;
        public int maxDropGold;

        public int GetRandomGoldAmount()
        => Random.Range(minDropGold, maxDropGold + 1);
    }
}
