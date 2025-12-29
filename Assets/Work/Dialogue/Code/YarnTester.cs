using UnityEngine;
using Yarn.Unity;

namespace Work.Dialogue
{
    public class YarnTester : MonoBehaviour
    {
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private string startNodeName = "Start";

        [ContextMenu("Start Dialogue")]
        private void StartDialogue()
        {
            dialogueRunner.StartDialogue(startNodeName);
        }
    }
}