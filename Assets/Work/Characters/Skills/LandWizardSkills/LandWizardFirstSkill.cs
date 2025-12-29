using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Characters.Code;
using Work.Characters.Skills.Code;
using Work.Combat;

namespace Work.Characters.Skills.LandWizardSkills
{
    public class LandWizardFirstSkill : AbstractCharacterSkill
    {
        public LandWizardFirstSkill(Character character, SkillData parameters) : base(character, parameters)
        {
        }

        public override void UseSkill()
        {
            base.UseSkill();

            
        }
    }
}
