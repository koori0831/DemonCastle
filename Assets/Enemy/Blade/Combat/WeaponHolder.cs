using Blade.Entities;
using UnityEngine;

namespace Blade.Combat
{
    public class WeaponHolder : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        [SerializeField] private Weapon[] weapons;
        //아까 가산점 못먹은 사람 기준으로 사망시 무기를 떨구도록 이 코드를 작성하세요.
        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void DropWeapons()
        {
            foreach (Weapon weapon in weapons)
            {
                weapon.Drop();
            }
        }
    }
}