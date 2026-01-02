using Blade.Core;
using Blade.Entities;
using Blade.Events;
using UnityEngine;

namespace Blade.Items
{
    public class ItemDropper : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private DropTableSO dropTable;
        [SerializeField] private GameObject goldBagPrefab;
        [SerializeField] private GameEventChannelSO playerChannel;

        private Entity _entity;
        private EntityActionData _actionData;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _actionData = entity.GetCompo<EntityActionData>();
        }

        public void DropItem()
        {
            int goldAmount = dropTable.GetRandomGoldAmount();
            
            GameObject goldBagObject = Instantiate(goldBagPrefab, _actionData.HitPoint, Quaternion.identity);
            GoldBag goldBag = goldBagObject.GetComponent<GoldBag>();
            Debug.Assert(goldBag != null, "Gold bag script not found check prefab");
            goldBag.SetGoldAmount(goldAmount);

            Vector3 dropForce = _actionData.HitNormal * -5f + Vector3.up * 4f;
            goldBag.AddForceToGoldBag(dropForce);
            
            playerChannel.RaiseEvent(PlayerEvents.AddExpEvent.Initializer(dropTable.dropExp));
        }
    }
}