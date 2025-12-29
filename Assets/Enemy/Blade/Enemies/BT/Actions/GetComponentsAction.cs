using System;
using System.Collections.Generic;
using Blade.Entities;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace Blade.Enemies.BT.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetComponents", story: "Get components from [Self]", category: "Enemy/GetCompo", id: "6140dfdf848debf13fcb91444fe59a5e")]
    public partial class GetComponentsAction : Action
    {
        [SerializeReference] public BlackboardVariable<Enemy> Self;

        protected override Status OnStart()
        {
            Enemy enemy  = Self.Value;

            List<BlackboardVariable> varList = enemy.BTAgent.BlackboardReference.Blackboard.Variables;

            foreach (var variable in varList)
            {
                if(typeof(IEntityComponent).IsAssignableFrom(variable.Type) == false) continue;
                SetVariable(enemy, variable.Name, enemy.GetCompo(variable.Type));
            }
            // SetVariable(enemy, "MainAnimator", enemy.GetCompo<EntityAnimator>());
            // SetVariable(enemy, "NavMovement", enemy.GetCompo<NavMovement>());
            //나중에 필요한 변수들을 여기다가 작성해주면 된다.
            return Status.Success;
        }

        private void SetVariable<T>(Enemy enemy, string varName, T component)
        {
            Debug.Assert(component != null , $"Check {varName} component exist on {enemy.gameObject.name}");
            if (enemy.BTAgent.GetVariable(varName, out BlackboardVariable target))
            {
                target.ObjectValue = component;
            }
            // BlackboardVariable<T> variable = enemy.GetBlackboardVariable<T>(varName);
            // variable.Value = component;
        }
    }
}

