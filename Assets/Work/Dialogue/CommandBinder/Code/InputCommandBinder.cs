using System.Collections;
using UnityEngine;
using Work.Characters.Events;
using Work.Utils.EventBus;
using Yarn.Unity;

namespace Work.Dialogue.CommandBinder.Code
{
	public class InputCommandBinder: MonoBehaviour
	{
        [SerializeField] private DialogueRunner dialogueRunner;

        private void Awake()
        {
            dialogueRunner.AddCommandHandler<bool>("InputEnabled", InputEnabledCommand);
        }

        private void InputEnabledCommand(bool enabled)
        {
            Bus<CharacterInputEnableEvent>.Raise(new CharacterInputEnableEvent(enabled));
        }
    }
}