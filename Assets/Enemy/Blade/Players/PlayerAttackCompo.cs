using Blade.Combat;
using Blade.Entities;
using Blade.StatSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Blade.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInitialize
    {
        [Header("Impulse settings")] 
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private bool canImpulseOnlyHit = true;
        
        [SerializeField] private AttackDataSO[] attackDataList;
        [SerializeField] private float _comboWindow;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private StatSO attackSpeedStat;
        [SerializeField] private StatSO meleeDamageStat;
        
        private Entity _entity;
        private EntityAnimator _entityAnimator;
        private EntityVFX _entityVFX;
        private EntityAnimatorTrigger _animatorTrigger;
        private EntityStatCompo _statCompo;
        private DamageCompo _damageCompo;

        private readonly int _attackSpeedHash = Animator.StringToHash("ATTACK_SPEED");
        private readonly int _comboCounterHash = Animator.StringToHash("COMBO_COUNTER");

        private float _attackSpeed = 1f; //나중에 스탯으로 분리합니다. 
        private float _lastAttackTime;
        public int ComboCounter { get; set; } = 0;
        public bool useMouseDirection;
        
        public float AttackSpeed
        {
            get => _attackSpeed;
            set
            {
                _attackSpeed = value;
                _entityAnimator.SetParam(_attackSpeedHash, _attackSpeed);
            }
        }
        
        public void Initialize(Entity entity)
        {
            _entity = entity;    
            _entityAnimator = entity.GetCompo<EntityAnimator>();
            _entityVFX = entity.GetCompo<EntityVFX>();
            _animatorTrigger = entity.GetCompo<EntityAnimatorTrigger>();
            _statCompo = entity.GetCompo<EntityStatCompo>();
            _damageCompo = entity.GetCompo<DamageCompo>();
        }
        
        public void AfterInitialize()
        {
            _animatorTrigger.OnAttackVFXTrigger += HandleAttackVFXTrigger;
            _animatorTrigger.OnDamageCastTrigger += HandleDamageCastTrigger;
            
            AttackSpeed = _statCompo.SubscribeStat(attackSpeedStat, HandleAttackSpeedChange, 1f);
        }

        private void HandleAttackSpeedChange(StatSO stat, float currentvalue, float previousvalue)
            => AttackSpeed = currentvalue;

        private void OnDestroy()
        {
            _statCompo.UnSubscribeStat(attackSpeedStat, HandleAttackSpeedChange);
            
            _animatorTrigger.OnAttackVFXTrigger -= HandleAttackVFXTrigger;
            _animatorTrigger.OnDamageCastTrigger -= HandleDamageCastTrigger;
        }

        private void HandleDamageCastTrigger()
        {
            AttackDataSO currentAttack = GetCurrentAttackData();
            DamageData damageData =
                _damageCompo.CalculateDamage(_statCompo.GetStat(meleeDamageStat), currentAttack);
            
            Vector3 position = damageCaster.transform.position;
            bool isSuccess = damageCaster.CastDamage(damageData, position, _entity.transform.forward, currentAttack);

            if (currentAttack.isPowerAttack && (canImpulseOnlyHit == false || isSuccess))
            {
                impulseSource.GenerateImpulse(currentAttack.impulseForce);
            }
            
        }

        private void HandleAttackVFXTrigger()
        {
            _entityVFX.PlayVFX($"Blade{ComboCounter}", Vector3.zero, Quaternion.identity);
        }

        public void Attack()
        {
            bool comboCounterOver = ComboCounter > 2;
            bool comboWindowExhaust = Time.time >= _lastAttackTime + _comboWindow;
            if(comboCounterOver || comboWindowExhaust)
                ComboCounter = 0; //콤보시간이 지났거나, 콤보 카운트가 토탈 콤보 갯수를 넘어갔다면
            
            _entityAnimator.SetParam(_comboCounterHash, ComboCounter);
        }

        public void EndAttack()
        {
            ComboCounter++;
            if (ComboCounter > 2) ComboCounter = 0;
            _lastAttackTime = Time.time; 
        }

        public AttackDataSO GetCurrentAttackData()
        {
            Debug.Assert(attackDataList.Length > ComboCounter, "Combo counter is out of range.");
            return attackDataList[ComboCounter];
        }

        
    }
}