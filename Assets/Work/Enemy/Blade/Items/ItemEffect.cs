using UnityEngine;

namespace Blade.Items
{
    public class ItemEffect : MonoBehaviour
    {
        public void SetItemEffect(bool isActive)
        {
            gameObject.SetActive(isActive);
            if(isActive)
                transform.rotation = Quaternion.identity;
        }
    }
}