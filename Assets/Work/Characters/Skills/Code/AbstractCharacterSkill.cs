using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Work.Characters.Code;
using Work.Combat;

namespace Work.Characters.Skills.Code
{
    public abstract class AbstractCharacterSkill
    {
        protected Character _owner;
        protected SkillData _skillData;

        private float _skillActiveTime = 0;

        public AbstractCharacterSkill(Character character, SkillData parameters)
        {
            _owner = character;
            _skillData = parameters;

        }

        public virtual void UseSkill()
        {
            _skillActiveTime = UnityEngine.Time.time;
        }

        public bool CanUseSkill()
        {
            if(_skillActiveTime + _skillData.cooldown <= UnityEngine.Time.time)
            {
                return true;
            }
            return false;
        }
    }
}
