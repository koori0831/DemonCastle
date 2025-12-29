using System.Collections;
using UnityEngine;
using Work.Interact.Code;
using Yarn.Unity;

namespace Work.Dialogue.Code
{
    public class DialogueObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private string startNodeName = "Start";

        public void Interact()
        {
            if (dialogueRunner.IsDialogueRunning)
            {
                Debug.Log("Dialogue is already running.");
                return;
            }
            dialogueRunner.StartDialogue(startNodeName);
        }

        public void SetInteractable(bool isInteractable)
        {
            Debug.Log($"DialogueObject {(isInteractable ? "is now" : "is no longer")} interactable.");
        }
    }
}