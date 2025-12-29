using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Work.Entities.Code;

namespace Work.Combat.Projectiles
{
    public class RockBullet : Projectile
    {
        public UnityEvent OnTakeDown;
        [SerializeField] private ParticleSystem destroyEffect;
        [SerializeField] private LayerMask damageableLayer;
        [SerializeField] private Color gizmoColor = new Color(1f, 0.2f, 0.2f, 0.35f);
        [SerializeField] private Color gizmoWireColor = Color.red;
        [SerializeField] private Color gizmoHitPointColor = Color.yellow;
        private float _radius = 6f;
        private float _damage;
        private Collider[] result;

        protected override void OnCollisionAfter(Collision collision)
        {
            result = Physics.OverlapSphere(transform.position, _radius, damageableLayer);
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
                {
                    Vector3 normal = transform.position - collision.collider.ClosestPoint(transform.position);
                    normal.x *= -1;
                    normal.z *= -1;
                    damageable.TakeDamage(_owner, _damage, normal.normalized, true, 250);
                }
            }

            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            //base.OnCollisionAfter(collision);
            GetComponent<Collider>().enabled = false;
            StartCoroutine(DeadProjectile());
            OnTakeDown?.Invoke();
        }

        public IEnumerator DeadProjectile()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }

        public void SetDamage(float value) => _damage = value;
        public void SetRadius(float value) => _radius = value;

        // 기즈모: 선택 시 범위와 영향을 받는 콜라이더를 시각화
        private void OnDrawGizmosSelected()
        {
            // 기본 와이어 구
            Gizmos.color = gizmoWireColor;
            Gizmos.DrawWireSphere(transform.position, _radius);

            // 반투명 채움 구 (Gizmos는 알파를 완벽히 지원하지 않을 수 있으나 시각적 보조로 사용)
            Color prev = Gizmos.color;
            Gizmos.color = gizmoColor;
            // DrawSphere는 실 체적을 그림
            Gizmos.DrawSphere(transform.position, _radius * 0.02f); // 중심 강조용으로 작게 그림
            Gizmos.color = prev;

            // 충돌체를 보여주기 위해 OverlapSphereNonAlloc 사용 (할당 최소화)
            const int MAX_HITS = 64;
            Collider[] hits = new Collider[MAX_HITS];
            int count = Physics.OverlapSphereNonAlloc(transform.position, _radius, hits);

            for (int i = 0; i < count; i++)
            {
                Collider c = hits[i];
                if (c == null) continue;

                // 충돌체의 발사체와 가장 가까운 지점
                Vector3 closest = c.ClosestPoint(transform.position);

                // 중심 -> 접점 선 (영향 방향 표시)
                Gizmos.color = gizmoHitPointColor;
                Gizmos.DrawLine(transform.position, closest);

                // 접점 위치에 작은 구체 표시
                Gizmos.DrawSphere(closest, 0.08f);

                // 노말(발사체 중심에서 접점으로 향한 벡터의 반대 방향)
                Vector3 normal = (transform.position - closest).normalized;
                if (normal.sqrMagnitude > 0f)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(closest, closest + normal * 0.5f);
                }
            }
        }
    }

}
