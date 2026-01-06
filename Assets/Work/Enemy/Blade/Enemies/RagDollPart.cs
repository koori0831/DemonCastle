using UnityEngine;

namespace Blade.Enemies
{
    public class RagDollPart : MonoBehaviour
    {
        private Rigidbody _rbCompo;
        private Collider _colliderCompo;

        public void InitializePart()
        {
            _rbCompo = GetComponent<Rigidbody>();
            _colliderCompo = GetComponent<Collider>();
        }

        public void SetRagDollActive(bool isActive)
        {
            _rbCompo.isKinematic = !isActive; //반대임
        }

        public void SetCollider(bool isActive)
        {
            _colliderCompo.enabled = isActive;
        }

        public async void KnockBack(Vector3 force, Vector3 position)
        {
            await Awaitable.FixedUpdateAsync(); //1 물리 프레임만큼 기다렸다가 들어가야한다.
            _rbCompo.AddForceAtPosition(force, position, ForceMode.Impulse);
        }
    }
}