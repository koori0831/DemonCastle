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
            parameters.GetValue("Bullet", out _sendBullet);

            _sensor = _owner.GetCompo<DetectSensorCompo>(true);
        }

        public override void Attack()
        {
            _params.GetValue("PositionOffset", out Vector3 offset);
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
