using System;
using System.Collections.Generic;
using UnityEngine;
using Work.Characters.Attacks.Code;
using Work.Characters.FSM.Code;
using Work.Combat;
using Work.Entities;
using Work.Utils.Helpers;

namespace Work.Characters.Code
{
    public class CharacterAttackCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; private set; }
        public bool IsCanAttack { get; private set; } = true;
        public bool isAttacking;


        private Character _character;
        private StateCompo _stateCompo;

        private AttackDataSO[] _attackDatas;
        private Dictionary<string, AbstractCharacterAttack> _attacks = new Dictionary<string, AbstractCharacterAttack>();
        public AbstractCharacterAttack CurrentAttack => _attacks[_attackDatas[CurrentAttackCount].AttackName];

        private CharacterAnimatorCompo _animatorCompo;

        private const float COMBO_DELAY = 0.35f;
        private float timer;

        private int _currentAttackCount;
        public int CurrentAttackCount { get { return _currentAttackCount; } }


        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _character = Owner as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _animatorCompo = _character.GetCompo<CharacterAnimatorCompo>(true);

            _attackDatas = _character.CharacterData.attackDatas;

            foreach (AttackDataSO item in _attackDatas)
            {
                Type type = Type.GetType(item.AttackClassPath);
                Debug.Assert(type != null, $"Type '{item.AttackClassPath}' not found.");
                AbstractCharacterAttack stateInstance = (AbstractCharacterAttack)Activator.CreateInstance(type, _character, item.Params);
                _attacks.Add(item.AttackName, stateInstance);
            }

            //이거 애니메이션 트리거에서 좌클릭으로 바꾸면 공격은 매끄럽게 나올듯 , + 일반공격 스크립트들에서 공격대상으로 Target 잡는거 따로 마우스 방향으로 바꿔야할듯
            ClickHelper.OnAttackTriggerEvent += Attack;
        }

        private void OnDestroy()
        {
            ClickHelper.OnAttackTriggerEvent -= Attack;
        }

        public void AttackStateChange()
        {
            if (IsCanAttack)
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
            //클릭했을때 바로 들어오는곳
            if (isAttacking) { return; } //현재 공격중이라면 리턴
            
            if (timer >= COMBO_DELAY)
            {
                _currentAttackCount = 0;
            }
            
            AttackStateChange();
            CurrentAttack.Attack();
        }

        private void Update()
        {
            if (!isAttacking)
            {
                timer += Time.deltaTime;
            }
            else
                timer = 0;
        }

        public void SetCanAttack() => IsCanAttack = true;
        public void SetAttackComboAnim() => _animatorCompo.SetParam(Animator.StringToHash("ATTACK_COUNT"), (float)_currentAttackCount);
    }
}