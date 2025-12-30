using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Work.Characters.Attacks.Code;
using Work.Characters.Events;
using Work.Characters.FSM.Code;
using Work.Characters.Skills.Code;
using Work.Combat;
using Work.Entities;
using Work.Utils.EventBus;

namespace Work.Characters.Code
{
    public class CharacterSkillCompo : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; set; }
        private Character _character;
        private CharacterMovementCompo _mover;
        private StateCompo _stateCompo;

        private Dictionary<string, AbstractCharacterSkill> skills = new Dictionary<string, AbstractCharacterSkill>();


        public void InitCompo(Entity entity)
        {
            Owner = entity;
            _character = entity as Character;
            _stateCompo = _character.GetCompo<StateCompo>();
            _mover = _character.GetCompo<CharacterMovementCompo>();

            foreach (SkillData item in _character.CharacterData.skillDatas)
            {
                Type type = Type.GetType(item.SkillClassPath);
                Debug.Assert(type != null, $"Type '{item.SkillClassPath}' not found.");
                AbstractCharacterSkill stateInstance = (AbstractCharacterSkill)Activator.CreateInstance(type, _character, item);
                skills.Add(item.SkillName, stateInstance);
            }

            Bus<CharacterSkillEvent>.Events += UseSkill;
            Bus<CharacterUltimateSkillEvent>.Events += UseUltimateSkill;
            Bus<CharacterDashEvent>.Events += UseDash;
        }

        private void OnDestroy()
        {
            Bus<CharacterSkillEvent>.Events -= UseSkill;
            Bus<CharacterUltimateSkillEvent>.Events -= UseUltimateSkill;
            Bus<CharacterDashEvent>.Events -= UseDash;
        }

        private void UseDash(CharacterDashEvent evt)
        {
            Debug.Assert(skills["Dash"] != null, "Dash Skill not find");

            if(_mover.IsCanDash && skills["Dash"].CanUseSkill())
            {
                Debug.Log("Dash");
                _stateCompo.ChangeState("DASH", true);
            }
        }

        private void UseUltimateSkill(CharacterUltimateSkillEvent evt)
        {
            Debug.Log($"CharacterSkillComponent: UseUltimateSkill by {Owner.name}");
        }

        private void UseSkill(CharacterSkillEvent evt)
        {
            Debug.Log($"CharacterSkillComponent: UseSkill {evt.skillNumber} by {Owner.name}");
        }


        public void UseSkill(string skillName)
        {
            if(skills.TryGetValue(skillName, out var skill))
            {
                skill.UseSkill();
            }
        }    
    }
}
