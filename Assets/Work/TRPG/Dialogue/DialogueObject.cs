using UnityEngine;

namespace Work.TRPG.Dialogue
{
    [RequireComponent(typeof(Collider))]
    public class DialogueObject : MonoBehaviour
    {
        [SerializeField] private DialogueContainerSO dialogueAsset;
        [SerializeField] private DialogueManager dialogueManager;

        [ContextMenu("Trigger Dialogue")]
        public void TriggerDialogue()
        {
            if (dialogueManager == null)
            {
                dialogueManager = FindAnyObjectByType<DialogueManager>();
            }

            if (dialogueManager == null || dialogueAsset == null)
            {
                Debug.LogWarning("DialogueObject: Missing manager or dialogue asset");
                return;
            }

            dialogueManager.StartDialogue(dialogueAsset);
        }

        private void OnTriggerEnter(Collider other)
        {
            TriggerDialogue();
        }
    }
}
