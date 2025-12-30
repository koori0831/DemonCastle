using UnityEngine;

namespace Blade.Enemies
{
    public class WayPoints : MonoBehaviour
    {
        private void Awake()
        {
            transform.SetParent(null);
        }
    }
}