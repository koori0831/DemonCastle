using Blade.Combat;
using Blade.Effects;
using UnityEngine;

namespace Blade.SkillSystem
{
    public class RollingSkill : Skill
    {
        [field: SerializeField] public MovementDataSO MovementData { get; private set; }
        [field: SerializeField] public PlayParticleVFX RollingVFX { get; private set; }
        
        public override void UseSkill()
        {
            base.UseSkill();
            
            RollingVFX.PlayVFX(RollingVFX.transform.position, transform.rotation);
        }
    }
}