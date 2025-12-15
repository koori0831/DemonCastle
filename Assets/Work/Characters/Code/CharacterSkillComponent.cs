using UnityEngine;
using Work.Characters.Events;
using Work.Entities;
using Work.Utils.EventBus;

namespace Work.Characters.Code
{
    public class CharacterSkillComponent : MonoBehaviour, IEntityComponent
    {
        public Entity Owner { get; set; }

        public void InitCompo(Entity entity)
        {
            Owner = entity;

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
            Debug.Log($"CharacterSkillComponent: UseDash by {Owner.name}");
        }

        private void UseUltimateSkill(CharacterUltimateSkillEvent evt)
        {
            Debug.Log($"CharacterSkillComponent: UseUltimateSkill by {Owner.name}");
        }

        private void UseSkill(CharacterSkillEvent evt)
        {
            Debug.Log($"CharacterSkillComponent: UseSkill {evt.skillNumber} by {Owner.name}");
        }



    }
}
