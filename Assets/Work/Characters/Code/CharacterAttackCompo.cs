using UnityEngine;
using Work.Characters.FSM.Code;
using Work.Combat;
using Work.Entities;
using Work.Entities.Code;

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
        private AbstractAttackDataSO[] attackDatas;
        private DetectSensorCompo _sensorCompo;
        private CharacterAnimatorCompo _animatorCompo;

        private const float ATTACK_DELAY = 0.3f;
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
            _sensorCompo.OnTargetChangedEvent += HandleTargetChangeEvent;

            attackDatas = _character.CharacterData.attackDatas;
        }

        private void HandleTargetChangeEvent(IDamageable currentTarget, IDamageable prev)// 타겟이 바뀌면 알려주는 함수
        {
            IsCanAttack = currentTarget != null;
            _currentAttackCount = 0;
            if (IsSoptAttack || !IsCanAttack)
                _stateCompo.ChangeState("MOVE", false);
            Attack();
        }

        public void Attack()
        {
            if (IsCanAttack && !IsSoptAttack)
            {
                SetAttackComboAnim();
                _stateCompo.ChangeState("ATTACK", true);
                _currentAttackCount = _currentAttackCount + 1 < attackDatas.Length ? _currentAttackCount + 1 : 0;
            }
        }

        public void Update()
        {
            if (!isAttacking)
            {
                timer += Time.deltaTime;

                if (timer >= ATTACK_DELAY)
                {
                    Attack();
                    timer = 0;
                }
            }
        }

        public void StopAttack() => IsSoptAttack = true;
        public void SetAttackComboAnim() => _animatorCompo.SetParam(Animator.StringToHash("ATTACK_COUNT"), (float)_currentAttackCount);
    }
}