using UnityEngine;
using Work.Characters.Attacks.Code;
using Work.Characters.Code;
using Work.Combat;
using Work.Combat.Projectiles;
using static UnityEngine.UI.GridLayoutGroup;

namespace Work.Characters.Attacks.LandWizardAttacks.Code
{
    public class LandWizardFirstAttack : AbstractCharacterAttack
    {
        private GameObject _sendBullet;
        private DetectSensorCompo _sensor;

        public LandWizardFirstAttack(Character character, AttackParameters parameters) : base(character, parameters)
        {
            _sendBullet = parameters.GetObjectValue("Bullet");

            _sensor = _owner.GetCompo<DetectSensorCompo>(true);
        }

        public override void Attack()
        {
            Vector3 offset = _params.GetVectorValue("PositionOffset");
            Vector3 calculateOffset = Quaternion.AngleAxis(_owner.transform.eulerAngles.y, Vector3.up) * offset;

            SendBullet bullet = MonoBehaviour.Instantiate(_sendBullet, _owner.transform.position + calculateOffset, Quaternion.identity).GetComponent<SendBullet>();
            Vector3 targetPos = _sensor.CurrentTarget.Transform.position;
            Vector3 dir = targetPos - bullet.transform.position;
            bullet.SetCanMove(true);
            bullet.SetDamage(_params.GetFloatValue("BulletDamage"));
            bullet.ProjectileInit(_owner, dir.normalized, _params.GetFloatValue("BulletSpeed"));
        }
    }
}
