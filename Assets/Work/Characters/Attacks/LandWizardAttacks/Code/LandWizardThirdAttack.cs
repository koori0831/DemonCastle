using UnityEngine;
using Work.Characters.Attacks.Code;
using Work.Characters.Code;
using Work.Combat;

namespace Work.Characters.Attacks.LandWizardAttacks.Code
{
    public class LandWizardThirdAttack : AbstractCharacterAttack
    {
        private GameObject _sendBullet;
        private DetectSensorCompo _sensor;

        public LandWizardThirdAttack(Character character, AttackParameters parameters) : base(character, parameters)
        {
        }

        public override void Attack()
        {

        }
    }
}
