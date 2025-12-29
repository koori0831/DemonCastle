using UnityEngine;

namespace Blade.Combat
{
    public interface IKnockBackable
    {
        void KnockBack(Vector3 direction, MovementDataSO kbMovement);
    }
}