using UnityEngine;
using System.Collections;
using Yarn.Unity;

namespace Work.TRPG.Code
{
	public class DiceRollPresenter: MonoBehaviour
	{
		[SerializeField] private DiceRollView diceRollView;

		private readonly DiceRollModel _model;
		private readonly VariableStorageBehaviour _variableStorage;
		private readonly DialogueRunner _dialogueRunner;

        public void OnEnable()
        {
        }
    }
}