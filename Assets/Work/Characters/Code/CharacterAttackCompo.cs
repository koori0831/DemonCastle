using System.Collections;
using System.Linq;
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

       
        private Character _character;
        private StateCompo _stateCompo;
        private AttackDataSO[] attackDatas;
        private DetectSensorCompo _sensorCompo;

        private int _currentAttackCount;
        public int CurrentAttackCount { get { return _currentAttackCount; } }


        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _character = Owner as Character;
            _sensorCompo = _character.GetCompo<DetectSensorCompo>();
            _stateCompo = _character.GetCompo<StateCompo>();
            _sensorCompo.OnTargetChangedEvent += HandleTargetChangeEvent;
            
            attackDatas = _character.CharacterData.attackDatas;
        }

        private void HandleTargetChangeEvent(IDamageable currentTarget, IDamageable prev)
        {
            Debug.Log("HandleTargetChangeEvent");
            IsCanAttack = _sensorCompo.IsExistTarget;
            if (IsCanAttack)
            {
                _currentAttackCount = 0;
                _stateCompo.ChangeState("ATTACK", true);
                StopAllCoroutines();
                StartCoroutine(AttackDelay());
            }
        }

        private IEnumerator AttackDelay()
        {
            yield return new WaitForSeconds(attackDatas[_currentAttackCount].AttackDelay);
            if (IsCanAttack)
            {
                _currentAttackCount++;
                if (_currentAttackCount >= attackDatas.Length)
                    _currentAttackCount = 0;
                _stateCompo.ChangeState("ATTACK", true);
                StartCoroutine(AttackDelay());
            }
        }

        public void StopAttack() => IsCanAttack = false;
    }
}