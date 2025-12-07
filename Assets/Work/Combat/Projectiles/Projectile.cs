using NUnit.Framework.Constraints;
using UnityEngine;

namespace Work.Combat.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody rbCompo;
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private LayerMask targetLayer;
        private const float STOP_OFFSET = 10f;
        private Vector3 _direction;
        private bool _isCanMove = false;

        public void SetDirection(Vector3 dir) => _direction = dir;

        public void SetCanMove(bool isCanMove = true) => _isCanMove = isCanMove;

        private void FixedUpdate()
        {
            Move();
        }


        private void Move()
        {
            if (_isCanMove)
            {
                rbCompo.linearVelocity = _direction;
            }
            else
            {
                rbCompo.linearVelocity = Vector3.Lerp(rbCompo.linearVelocity,Vector3.zero,Time.deltaTime * STOP_OFFSET);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((collision.gameObject.layer & targetLayer) != 0)
            {
                OnCollisionAfter();
            }
        }

        protected virtual void OnCollisionAfter()
        {
            Destroy(gameObject);
        }
    }
}