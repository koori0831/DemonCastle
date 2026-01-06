using System;
using Unity.Behavior;
using UnityEngine;

namespace Blade.Enemies.BT.Conditions
{
    [Serializable, Unity.Properties.GeneratePropertyBag]
    [Condition(name: "TimerPass", story: "Check [Timer] pass [Sec]", category: "Conditions", id: "1e4a60003596d428821b4a3af83e1687")]
    public partial class TimerPassCondition : Condition
    {
        [SerializeReference] public BlackboardVariable<float> Timer;
        [SerializeReference] public BlackboardVariable<float> Sec;

        public override bool IsTrue()
        {
            return Timer + Sec < Time.time;
        }
    }
}
