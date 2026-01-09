using System.Collections;
using UnityEngine;
using Work.Cameras.Code;
using Work.Characters.Attacks.Code;
using Work.Characters.Code;
using Work.Combat;
using Work.Combat.Projectiles;
using Work.Utils.Helpers;

namespace Work.Characters.Attacks.LandWizardAttacks.Code
{
    public class LandWizardThirdAttack : AbstractCharacterAttack
    {
        private GameObject _rockObject;

        public LandWizardThirdAttack(Character character, DataParams parameters) : base(character, parameters)
        {
            parameters.GetValue("Rock",out _rockObject);
        }

        public override void Attack()
        {
            Vector3 targetPos = ClickHelper.LastClickData.Point;

            RockBullet bullet = MonoBehaviour.Instantiate(_rockObject, targetPos + new Vector3(Random.Range(-0.25f,0.25f),5, Random.Range(-0.25f, 0.25f)), Quaternion.identity).GetComponent<RockBullet>();

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
