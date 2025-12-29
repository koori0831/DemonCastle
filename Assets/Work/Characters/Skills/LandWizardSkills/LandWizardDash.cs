using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Characters.Code;
using Work.Characters.FSM.Code;
using Work.Characters.Skills.Code;
using Work.Combat;

namespace Work.Characters.Skills.LandWizardSkills
{
    public class LandWizardDash : AbstractCharacterSkill
    {
        private CharacterMovementCompo _mover;
        private StateCompo _stateCompo;

        public LandWizardDash(Character character, SkillData parameters) : base(character, parameters)
        {
            _mover = _owner.GetCompo<CharacterMovementCompo>();
            _stateCompo = _owner.GetCompo<StateCompo>();
        }

        public override void UseSkill()
        {
            base.UseSkill();
            _mover.Dash();
        }
    }
}
