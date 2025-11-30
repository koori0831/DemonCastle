using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Utils.EventBus;

namespace Work.Utils.EventBus.Events
{
    public struct PlayerMoveEvent : IEvent
    {
        public Vector3 MoveDirection { get; private set; }

        public PlayerMoveEvent(Vector3 dir)
        {
            MoveDirection = dir;
        }
    }
}
