using Blade.Combat;
using Blade.Core;
using Blade.Entities;
using Blade.Events;
using Blade.FSM;
using Blade.SkillSystem;
using GondrLib.Dependencies;
using UnityEngine;

namespace Blade.Players
{
    
    public class Player : Entity, IDependencyProvider, IKnockBackable
    {
        [field: SerializeField] public GameEventChannelSO PlayerChannel { get; private set; }
        [field: SerializeField] public PlayerInputSO PlayerInputSo { get; private set; }
        [SerializeField] private StateSO[] states;

        public int Health { get; set; }
        [Provide]
        public Player GetPlayer() => this;

        private SkillComponent _skillComponent;
            
        #region temp region
        
        private EntityActionData _actionData;
        private CharacterMovement _movement;
        #endregion

        private EntityHealth _healthCompo;
        private EntityStateMachine _stateMachine;

        protected override void InitializeComponents()
        {
            base.InitializeComponents();
            _actionData = GetCompo<EntityActionData>();
            _movement = GetCompo<CharacterMovement>();
            _skillComponent = GetCompo<SkillComponent>();
            _healthCompo = GetCompo<EntityHealth>();
            
            _stateMachine = new EntityStateMachine(this, states);
        }

        protected override void AfterInitializeComponents()
        {
            base.AfterInitializeComponents();
            OnHitEvent.AddListener(HandleHitEvent);
            OnDieEvent.AddListener(HandleDeadEvent);
            PlayerInputSo.OnRollingPressed += HandleRollingKeyPressed;

            _healthCompo.OnHealthChange += HandleHealthChange;
        }

        private void OnDestroy()
        {
            PlayerInputSo.OnRollingPressed -= HandleRollingKeyPressed;
            OnHitEvent.RemoveListener(HandleHitEvent);
            OnDieEvent.RemoveListener(HandleDeadEvent);
            _healthCompo.OnHealthChange -= HandleHealthChange;
        }

        private void HandleHealthChange(float current, float max)
        {
            PlayerChannel.RaiseEvent(PlayerEvents.PlayerHealthEvent.Initializer(current, max));
        }

        private void HandleDeadEvent()
        {
            if (IsDead) return;
            IsDead = true;
            PlayerChannel.RaiseEvent(PlayerEvents.PlayerDead);
            ChangeState("DEAD", true);//강제로 전환
        }

        private void HandleHitEvent()
        {
            if (IsDead) return;
            if (_actionData.HitByPowerAttack)
            {
                const string hit = "HIT";
                ChangeState(hit, true);
            }
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }

        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }
        public void ChangeState(string newStateName, bool forced = false) 
            => _stateMachine.ChangeState(newStateName, forced);
        
        private void HandleRollingKeyPressed()
        {
            RollingSkill skill = _skillComponent.GetSkill<RollingSkill>();
            if (skill == null || skill.IsCooldown) return;
            
            skill.UseSkill();
            ChangeState("ROLLING");
        }

        public void KnockBack(Vector3 direction, MovementDataSO kbMovement)
        {
            _movement.KnockBack(direction, kbMovement);
        }
    }
}