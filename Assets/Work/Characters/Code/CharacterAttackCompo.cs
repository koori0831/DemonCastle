using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Work.Characters.Attacks.Code;
using Work.Characters.FSM.Code;
using Work.Combat;
using Work.Entities;
using Work.Entities.Code;
using static UnityEditor.VersionControl.Asset;

namespace Work.Characters.Code
{
    public class CharacterAttackCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public bool IsCanAttack { get; private set; }
        public bool IsSoptAttack { get; private set; } = false;
        public bool isAttacking;


        private Character _character;
        private StateCompo _stateCompo;

        private AbstractAttackDataSO[] _attackDatas;
        private Dictionary<string, AbstractCharacterAttack> _attacks = new Dictionary<string, AbstractCharacterAttack>();
        private AbstractCharacterAttack CurrentAttack => _attacks[_attackDatas[CurrentAttackCount].AttackName];

        private DetectSensorCompo _sensorCompo;
        private CharacterAnimatorCompo _animatorCompo;
        private CharacterAnimationTriggerCompo _animTriggerCompo;

        private const float ATTACK_DELAY = 0.5f;
        private float timer;

        private int _currentAttackCount;
        public int CurrentAttackCount { get { return _currentAttackCount; } }


        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _character = Owner as Character;
            _sensorCompo = _character.GetCompo<DetectSensorCompo>();
            _stateCompo = _character.GetCompo<StateCompo>();
            _animatorCompo = _character.GetCompo<CharacterAnimatorCompo>(true);
            _animTriggerCompo = _character.GetCompo<CharacterAnimationTriggerCompo>(true);
            _sensorCompo.OnTargetChangedEvent += HandleTargetChangeEvent;

            _attackDatas = _character.CharacterData.attackDatas;

            foreach (AbstractAttackDataSO item in _attackDatas)
            {
                Type type = Type.GetType(item.AttackClassPath);
                Debug.Assert(type != null, $"Type '{item.AttackClassPath}' not found.");
                AbstractCharacterAttack stateInstance = (AbstractCharacterAttack)Activator.CreateInstance(type, _character,item.AttackParams);
                _attacks.Add(item.AttackName, stateInstance);
            }

            _animTriggerCompo.OnAttackTriggerEvent += Attack;
        }

        private void OnDestroy()
        {
            _animTriggerCompo.OnAttackTriggerEvent -= Attack;
        }

        private void HandleTargetChangeEvent(IDamageable currentTarget, IDamageable prev)// 타겟이 바뀌면 알려주는 함수
        {
            IsCanAttack = currentTarget != null && currentTarget.Transform != null && currentTarget.Transform.gameObject != null;
            _currentAttackCount = 0;
            if (IsSoptAttack || !IsCanAttack)
                _stateCompo.ChangeState("IDLE", false);
            AttackStateChange();
        }

        public void AttackStateChange()
        {
            if (IsCanAttack && !IsSoptAttack)
            {
                SetAttackComboAnim();
                _stateCompo.ChangeState("ATTACK", true);
                
            }
        }

        public void AddAttackCount()
        {
            _currentAttackCount = _currentAttackCount + 1 < _attackDatas.Length ? _currentAttackCount + 1 : 0;
        }

        public void Attack()
        {
            CurrentAttack.Attack();
        }

        public void Update()
        {
            if (!isAttacking)
            {
                timer += Time.deltaTime;

                if (timer >= ATTACK_DELAY)
                {
                    AttackStateChange();
                    timer = 0;
                }
            }
        }

        public void StopAttack() => IsSoptAttack = true;
        public void SetAttackComboAnim() => _animatorCompo.SetParam(Animator.StringToHash("ATTACK_COUNT"), (float)_currentAttackCount);
    }
}