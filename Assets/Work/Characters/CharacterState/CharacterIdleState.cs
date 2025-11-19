using System.Collections;
using UnityEngine;
using Work.Characters.FSM.Code;
using Work.Entities;

namespace Work.Characters.CharacterState
{
    public class CharacterIdleState : State
    {
        public CharacterIdleState(Entity entity, EntityAnimatorCompo animator, int animHash) : base(entity, animator, animHash)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Idle 진입");
        }
    }
}