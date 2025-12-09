using System.Collections;
using UnityEngine;
using Work.Characters.Attacks.Code;
using Work.Characters.Code;
using Work.Combat;
using Work.Combat.Projectiles;

namespace Work.Characters.Attacks.LandWizardAttacks.Code
{
    public class LandWizardThirdAttack : AbstractCharacterAttack
    {
        private GameObject _rockObject;
        private DetectSensorCompo _sensor;

        public LandWizardThirdAttack(Character character, AttackParameters parameters) : base(character, parameters)
        {
            parameters.GetValue("Rock",out _rockObject);
            _sensor = character.GetCompo<DetectSensorCompo>();
        }

        public override void Attack()
        {
            _params.GetValue("PositionOffset",out Vector3 offset);
            offset.z = Vector3.Distance(_owner.transform.position, _sensor.CurrentTarget.Transform.position);
            Vector3 calculateOffset = Quaternion.AngleAxis(_owner.transform.eulerAngles.y, Vector3.up) * offset;

            RockBullet bullet = MonoBehaviour.Instantiate(_rockObject, _owner.transform.position + calculateOffset + new Vector3(Random.Range(-0.25f,0.25f),0, Random.Range(-0.25f, 0.25f)), Quaternion.identity).GetComponent<RockBullet>();
           
            bullet.SetDirection(Vector3.down * _params.GetFloatValue("Speed"));
            bullet.SetRadius(_params.GetFloatValue("Radius"));
            bullet.SetDamage(_params.GetFloatValue("Damage"));
            _owner.StartCoroutine(MoveRock(bullet));
        }

        public IEnumerator MoveRock(RockBullet bullet)
        {
            yield return new WaitForSeconds(_params.GetFloatValue("MoveTime"));
            bullet.SetCanMove(true);
        }
    }
}
