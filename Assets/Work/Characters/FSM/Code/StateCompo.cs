using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Work.Entities;

namespace Work.Characters.FSM.Code
{
    public class StateCompo : MonoBehaviour , IEntityComponent
    {
        public Entity Owner { get; protected set; }

        public void InitCompo(Entity entity)
        {
            Owner = entity;
        }
    }
}
