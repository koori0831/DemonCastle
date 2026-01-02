using Blade.Combat;
using Blade.Entities;
using Blade.UI;
using System;
using Unity.Behavior;
using UnityEngine;
using Work.Combat;

namespace Blade.Enemies
{
    public abstract class Enemy : Entity, Work.Entities.Code.IDamageable
    {
        [field: SerializeField] public EntityFinderSO PlayerFinder { get; private set; }
        [SerializeField] private NavMovement controller;
        [SerializeField] private DamageText damageTextPrefab;
        [SerializeField] private TextInfoSO normalText;

        public BehaviorGraphAgent BTAgent { get; private set; }

        public Transform Transform => this != null ? transform : null;

        public Action<Work.Entities.Code.IDamageable> OnDeadEvent { get; set; }


        #region Temp

        public float detectRange;
        public float attackRange;

        #endregion


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }

        protected override void AddComponents()
        {
            base.AddComponents();
            BTAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(BTAgent != null, $"{gameObject.name} doesn't have behavior graph agent");
        }

        protected virtual void Start()
        {
            BlackboardVariable<Transform> target = GetBlackboardVariable<Transform>("Target");
            target.Value = PlayerFinder.Target.transform;
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if (BTAgent.GetVariable(key, out BlackboardVariable<T> result))
                return result;
            return default;
        }
        public MovementDataSO data;
        public Rigidbody rbCompo;

        public float health = 1000;
        private bool _isKnockback = false;
        private float _timer;
        private const float DELAY_TIME = 0.1f;
        private void Update()
        {
            if (_isKnockback)
            {
                _timer += Time.deltaTime;
                if (_timer > DELAY_TIME)
                {
                    _isKnockback = false;
                    _timer = 0;
                }
            }
        }

        public void TakeDamage(Work.Entities.Entity attacker, float damage, Vector3 normal, bool isKnockback = false, float knockbackPower = 0)
        {
            health -= damage;
            DamageText damageText = Instantiate(damageTextPrefab, transform.position, Quaternion.identity);
            Vector3 pos = transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 2.5f, UnityEngine.Random.Range(-0.5f, 0.5f));
            damageText.Init($"{damage}", normalText, pos, 1.2f);

            if (isKnockback && !_isKnockback)
            {
                Debug.Log("Knockback");
                controller.KnockBack(normal, data);
            }

            if (health <= 0)
            {
                IsDead = true;
                OnDeadEvent?.Invoke(this);
                OnDieEvent?.Invoke();
                gameObject.SetActive(false);
                //Destroy(gameObject);
            }
        }
    }
}