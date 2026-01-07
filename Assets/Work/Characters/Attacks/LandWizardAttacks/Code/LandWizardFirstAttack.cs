using UnityEngine;
using Work.Cameras.Code;
using Work.Characters.Attacks.Code;
using Work.Characters.Code;
using Work.Combat;
using Work.Combat.Projectiles;
using Work.Entities.Code;

namespace Work.Characters.Attacks.LandWizardAttacks.Code
{
    public class LandWizardFirstAttack : AbstractCharacterAttack
    {
        private GameObject _sendBullet;
        private DetectSensorCompo _sensor;
        private CameraHandlerCompo _cameraHandle;

        public LandWizardFirstAttack(Character character, DataParams parameters) : base(character, parameters)
        {
            parameters.GetValue("Bullet", out _sendBullet);

            _sensor = _owner.GetCompo<DetectSensorCompo>(true);
            _cameraHandle = _owner.GetCompo<CameraHandlerCompo>();
        }

        public override void Attack()
        {

            Vector3 mousePos = Vector3.zero; //여기 마우스 포지션으로 바꿔야함

            _params.GetValue("PositionOffset", out Vector3 offset);
            _cameraHandle.GenerateImpulse("LandWizardFirstAttack");
            Vector3 calculateOffset = Quaternion.AngleAxis(_owner.transform.eulerAngles.y, Vector3.up) * offset;

            SendBullet bullet = MonoBehaviour.Instantiate(_sendBullet, _owner.transform.position + calculateOffset, Quaternion.identity).GetComponent<SendBullet>();
            Vector3 targetPos = mousePos + Vector3.up;
            Vector3 dir = targetPos - bullet.transform.position;
            bullet.SetCanMove(true);
            bullet.SetDamage(_params.GetFloatValue("BulletDamage"));
            bullet.ProjectileInit(_owner, dir.normalized, _params.GetFloatValue("BulletSpeed"));
        }
    }
}
