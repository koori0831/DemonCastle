using UnityEngine;
using Work.Entities;

namespace Work.Combat.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody rbCompo;
        [SerializeField] private LayerMask targetLayer;
        protected Entity _owner;
        protected float _bulletSpeed = 5f;
        protected const float STOP_OFFSET = 10f;
        protected Vector3 _direction;
        protected bool _isCanMove = false;

        public virtual void ProjectileInit(Entity owner, Vector3 dir, float speed)
        {
            _owner = owner;
            SetSpeed(speed);
            SetDirection(dir);
        }

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
                rbCompo.linearVelocity = _direction * _bulletSpeed;
            }
            else
            {
                rbCompo.linearVelocity = Vector3.Lerp(rbCompo.linearVelocity, Vector3.zero, Time.deltaTime * STOP_OFFSET);
            }
        }

        public void SetSpeed(float value)
        {
            _bulletSpeed = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnCollisionEnter(Collision collision)
        {
            if ((1 << collision.gameObject.layer & targetLayer) != 0)
            {
                OnCollisionAfter(collision);
                _isCanMove = false;
            }
        }

        protected virtual void OnCollisionAfter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}