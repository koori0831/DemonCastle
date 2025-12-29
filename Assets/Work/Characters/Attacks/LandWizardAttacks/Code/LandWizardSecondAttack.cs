using UnityEngine;
using Work.Cameras.Code;
using Work.Characters.Attacks.Code;
using Work.Characters.Code;
using Work.Combat;
using Work.Combat.Projectiles;

namespace Work.Characters.Attacks.LandWizardAttacks.Code
{
    public class LandWizardSecondAttack : AbstractCharacterAttack
    {
        private GameObject _sendBullet;
        private DetectSensorCompo _sensor;
        private CameraHandlerCompo _cameraHandle;

        public LandWizardSecondAttack(Character character, DataParams parameters) : base(character, parameters)
        {
            _sensor = _owner.GetCompo<DetectSensorCompo>(true);
            _cameraHandle = _owner.GetCompo<CameraHandlerCompo>();
            _sendBullet = parameters.GetObjectValue("Bullet");
        }

        public override void Attack()
        {
            Debug.Assert(_sensor.CurrentTarget.Transform.position != null, "Target null");
            if (_sensor.CurrentTarget.Transform.position == null)
                return;


            int bulletCount = _params.GetIntValue("BulletCount");
            _cameraHandle.GenerateImpulse("LandWizardSecondAttack");

            for (int i = -bulletCount / 2; i <= bulletCount / 2; i++)
            {
                float oneAngle = _params.GetFloatValue("BulletAngle") / bulletCount;


                _params.GetValue("PositionOffset",out Vector3 offset);
                Vector3 calculateOffset = Quaternion.AngleAxis(_owner.transform.eulerAngles.y, Vector3.up) * offset;

                SendBullet bullet = MonoBehaviour.Instantiate(_sendBullet, _owner.transform.position + calculateOffset, Quaternion.identity).GetComponent<SendBullet>();
                Vector3 targetPos = _sensor.CurrentTarget.Transform.position;
                Vector3 dir = targetPos - bullet.transform.position;
                Vector3 calDir = Quaternion.AngleAxis(oneAngle * i, Vector3.up) * dir;
                bullet.SetCanMove(true);
                bullet.SetDamage(_params.GetFloatValue("BulletDamage"));
                bullet.ProjectileInit(_owner,calDir.normalized, _params.GetFloatValue("BulletSpeed"));
            }
        }
    }
}
