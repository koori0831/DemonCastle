using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Work.Characters.FSM.Code
{
    public class StateMachine
    {
        private Dictionary<string,State> states = new Dictionary<string, State>();

        public State CurrentState { get; private set; }

        public void ChangeState(string stateName, bool isForcing = false)
        {
            if (CurrentState != null && !isForcing && CurrentState == states[stateName])
                return;

            if (states.ContainsKey(stateName))
            {
                CurrentState?.Exit();    
                CurrentState = states[stateName];
                CurrentState?.Enter();
            }
            else
            {
                throw new Exception($"State '{stateName}' not found in the state machine.");
            }
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}
