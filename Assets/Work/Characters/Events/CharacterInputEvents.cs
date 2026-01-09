using UnityEngine;
using Work.Utils.EventBus;
using Work.Utils.Helpers;

namespace Work.Characters.Events
{
    public class CharacterInputEvents { }

    public struct CharacterMoveEvent : IEvent
    {
        public Vector3 MoveDirection { get; private set; }

        public CharacterMoveEvent(Vector3 dir)
        {
            MoveDirection = dir;
        }
    }

    public struct CharacterSkillEvent : IEvent
    {
        public int skillNumber { get; private set; }

        public CharacterSkillEvent(int number)
        {
            skillNumber = number;
        }
    }

    public struct CharacterDashEvent : IEvent
    {

    }

    public struct CharacterUltimateSkillEvent : IEvent
    {

    }

    public struct CharacterInteractionEvent : IEvent
    {

    }
    public struct CharacterInputEnableEvent : IEvent
    {
        public bool Enable { get; private set; }
        public CharacterInputEnableEvent(bool enable)
        {
            Enable = enable;
        }
    }

    public struct MouseClickEvent : IEvent
    {
        public ClickData ClickData { get; private set; }

        public MouseClickEvent(ClickData clickData)
        {
            ClickData = clickData;
        }
    }
}
