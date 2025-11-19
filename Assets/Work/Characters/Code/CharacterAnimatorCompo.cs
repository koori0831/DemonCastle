using UnityEngine;
using Work.Entities;

namespace Work.Characters.Code
{
    public class CharacterAnimatorCompo : EntityAnimatorCompo
    {
        private Character _character;

        public override void InitCompo(Entity entity)
        {
            base.InitCompo(entity);
            _character = entity as Character;

            if (_character.CharacterData.AnimationData.AnimatorController != null 
                && _character.CharacterData.AnimationData.visualPrefab != null)
            {
                animator.runtimeAnimatorController = _character.CharacterData.AnimationData.AnimatorController as RuntimeAnimatorController;
                GameObject visual = Instantiate(_character.CharacterData.AnimationData.visualPrefab,transform);
            }


            //여기서 메쉬도 바뀌도록
        }
    }
}